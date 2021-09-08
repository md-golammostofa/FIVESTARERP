using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
    public class TransferInfoBusiness : ITransferInfoBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly TransferInfoRepository transferInfoRepository; // repo
        private readonly IMobilePartStockDetailBusiness _mobilePartStockDetailBusiness;
        private readonly TransferDetailRepository transferDetailRepository; // repo
        private readonly IMobilePartStockInfoBusiness _mobilePartStockInfoBusiness;

        private readonly ITransferDetailBusiness _transferDetailBusiness;
        public TransferInfoBusiness(IConfigurationUnitOfWork configurationDb, IMobilePartStockDetailBusiness mobilePartStockDetailBusiness, ITransferDetailBusiness transferDetailBusiness, IMobilePartStockInfoBusiness mobilePartStockInfoBusiness)
        {
            this._mobilePartStockDetailBusiness = mobilePartStockDetailBusiness;
            this._transferDetailBusiness = transferDetailBusiness;
            this._configurationDb = configurationDb;
            transferInfoRepository = new TransferInfoRepository(this._configurationDb);
            transferDetailRepository = new TransferDetailRepository(this._configurationDb);
            this._mobilePartStockInfoBusiness = mobilePartStockInfoBusiness;
        }

        public IEnumerable<TransferInfo> GetAllStockTransferByOrgId(long orgId)
        {
            return transferInfoRepository.GetAll(ts => ts.OrganizationId == orgId).ToList();
        }

        public IEnumerable<TransferInfo> GetAllStockTransferByOrgIdAndBranch(long orgId, long branchId)
        {
            return transferInfoRepository.GetAll(ts => ts.OrganizationId == orgId && ts.BranchId == branchId).ToList();
        }

        public TransferInfo GetStockTransferInfoById(long id, long orgId, long branchId)
        {
            return transferInfoRepository.GetOneByOrg(t => t.TransferInfoId == id && t.OrganizationId == orgId && t.BranchId == branchId);
        }

        public TransferInfo GetStockTransferInfoById(long id, long orgId)
        {
            return transferInfoRepository.GetOneByOrg(t => t.TransferInfoId == id && t.OrganizationId == orgId);
        }

        public TransferInfoDTO GetStockTransferInfoDataById(long id, long orgId)
        {
            return this._configurationDb.Db.Database.SqlQuery<TransferInfoDTO>(string.Format(@"Select ti.DescriptionId,ti.BranchId,ti.TransferInfoId,ti.TransferCode,ti.StateStatus,m.ModelName,b.BranchName From tblTransferInfo ti
Left Join tblModelSS m on ti.DescriptionId=m.ModelId
Left Join [ControlPanel].dbo.tblBranch b on ti.BranchId=b.BranchId
Where ti.TransferInfoId={0} and ti.OrganizationId={1}", id,orgId)).FirstOrDefault();
        }

        public bool ReceiveStockAndUpdateStatus(List<TransferDetailDTO> details, long userId, long orgId, long branchId)
        {
            bool IsSuccess = false;
            List<MobilePartStockDetailDTO> receiveStockInItems = new List<MobilePartStockDetailDTO>();
            var reqInfo = GetStockTransferInfoById(details.FirstOrDefault().TransferInfoId, orgId,branchId);
            if(reqInfo != null)
            {
                reqInfo.StateStatus = "Accepted";
                reqInfo.UpdateDate = DateTime.Now;
                reqInfo.UpUserId = userId;
            }
            transferInfoRepository.Update(reqInfo);
            foreach(var item in details)
            {
                var reqDetails = _transferDetailBusiness.GetOneByOneDetailId(item.TransferDetailId, orgId, branchId);
                MobilePartStockDetailDTO stockInItem = new MobilePartStockDetailDTO
                {
                    SWarehouseId = reqInfo.WarehouseId,
                    MobilePartId = item.PartsId,
                    CostPrice = reqDetails.CostPrice,
                    SellPrice = reqDetails.SellPrice,
                    Quantity = item.IssueQty,
                    Remarks = "Stock-In By Branch Requsition",
                    StockStatus = StockStatus.StockIn,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    BranchId = branchId,
                    ReferrenceNumber = reqInfo.TransferCode,
                    DescriptionId = reqInfo.DescriptionId
                };
                receiveStockInItems.Add(stockInItem);
            }
            if (transferInfoRepository.Save() == true)
            {
                IsSuccess = _mobilePartStockDetailBusiness.SaveMobilePartStockInByBranchRequsition(receiveStockInItems, userId, orgId, branchId);
            }
            return IsSuccess;
        }

        public bool SaveTransferInfoStateStatus(long transferId,long swarehouse, string status, long userId, long orgId, long branchId)
        {
            bool IsSuccess = false;
            var info = GetStockTransferInfoById(transferId, orgId);
            if (info != null && info.StateStatus == RequisitionStatus.Approved)
            {
                info.WarehouseIdTo = swarehouse;
                info.StateStatus = RequisitionStatus.Accepted;
                info.UpUserId = userId;
                info.UpdateDate = DateTime.Now;
                transferInfoRepository.Update(info);
                if (transferInfoRepository.Save())
                {
                    var details = _transferDetailBusiness.GetAllTransferDetailByInfoId(transferId,orgId);
                    if (details.Count() > 0)
                    {
                        List<MobilePartStockDetailDTO> stockDetails = new List<MobilePartStockDetailDTO>();
                        foreach (var item in details)
                        {
                            MobilePartStockDetailDTO detailItem = new MobilePartStockDetailDTO()
                            {
                                DescriptionId = item.DescriptionId,
                                BranchFrom = info.BranchId,
                                BranchId = info.BranchTo.Value,
                                SWarehouseId = swarehouse,
                                MobilePartId = item.PartsId,
                                StockStatus = StockStatus.StockIn,
                                Quantity = item.Quantity,
                                CostPrice=item.CostPrice,
                                SellPrice=item.SellPrice,
                                EUserId = userId,
                                EntryDate = DateTime.Now,
                                OrganizationId = orgId,
                                ReferrenceNumber = info.TransferCode,
                                Remarks = "Stock In By Another Branch ("+info.TransferCode+")",
                            };
                            stockDetails.Add(detailItem);
                        }
                        IsSuccess = _mobilePartStockDetailBusiness.SaveMobilePartStockIn(stockDetails, userId, orgId, info.BranchTo.Value);
                    }
                }
            }

            return IsSuccess;
        }

        public bool SaveTransferStockInfo(TransferInfoDTO info, List<TransferDetailDTO> detailDTO, long userId, long orgId, long branchId)
        {
            bool IsSuccess = false;
            TransferInfo transferInfo = new TransferInfo
            {
                DescriptionId = info.DescriptionId,
                TransferInfoId = info.TransferInfoId,
                BranchTo = info.BranchTo,
                BranchId = branchId,
                WarehouseId = info.SWarehouseId,
                OrganizationId = orgId,
                StateStatus = "Pending",
                Remarks = "",
                EUserId = userId,
                EntryDate = DateTime.Now,
                TransferCode = ("TSB-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"))
            };
            List<TransferDetail> details = new List<TransferDetail>();
            List<MobilePartStockDetailDTO> TransferStockOutItems = new List<MobilePartStockDetailDTO>();

            foreach (var item in detailDTO)
            {
                TransferDetail detail = new TransferDetail
                {
                    PartsId = item.PartsId,
                    //CostPrice=item.CostPrice,
                    //SellPrice=item.SellPrice,
                    Quantity = item.Quantity,
                    Remarks = item.Remarks,
                    BranchTo=info.BranchTo,
                    BranchId= branchId,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    DescriptionId = info.DescriptionId
                };
                details.Add(detail);
                //MobilePartStockDetailDTO stockOutItem = new MobilePartStockDetailDTO
                //{
                //    SWarehouseId = info.SWarehouseId,
                //    MobilePartId = item.PartsId,
                //    CostPrice=item.CostPrice,
                //    SellPrice=item.SellPrice,
                //    Quantity = item.Quantity,
                //    Remarks = transferInfo.TransferCode,
                //    StockStatus = StockStatus.StockOut,
                //    OrganizationId = orgId,
                //    EntryDate = DateTime.Now,
                //    EUserId = userId,
                //    BranchId = branchId,
                //    ReferrenceNumber= transferInfo.TransferCode,
                //    DescriptionId = info.DescriptionId //Nishad
                //};
                //TransferStockOutItems.Add(stockOutItem);
            }

            transferInfo.TransferDetails = details;
            transferInfoRepository.Insert(transferInfo);

            if (transferInfoRepository.Save())
            {
                //IsSuccess = _mobilePartStockDetailBusiness.SaveMobilePartStockOut(TransferStockOutItems, userId, orgId, branchId);
                IsSuccess = true;
            }
            return IsSuccess;
        }

        public bool UpdateTransferStatusAndStockOut(TransferInfoDTO dto, long orgId, long branchId, long userId)
        {
            bool IsSuccess = false;
            List<TransferDetail> detailslist = new List<TransferDetail>();
            List<MobilePartStockDetailDTO> TransferStockOutItems = new List<MobilePartStockDetailDTO>();
            var reqInfo = GetStockTransferInfoById(dto.TransferInfoId, orgId);
            if(reqInfo != null)
            {
                reqInfo.StateStatus = dto.StateStatus;
                reqInfo.UpdateDate = DateTime.Now;
                reqInfo.UpUserId = userId;
                transferInfoRepository.Update(reqInfo);
                
            }
            foreach(var item in dto.TransferDetails)
            {
                var partsPrice = _mobilePartStockInfoBusiness.GetPriceByModel(dto.DescriptionId.Value, item.PartsId.Value, orgId, branchId);
                var reqDetails = _transferDetailBusiness.GetOneByDetailId(item.TransferDetailId, orgId, branchId);
                if(reqDetails != null)
                {
                    reqDetails.CostPrice = partsPrice.CostPrice;
                    reqDetails.SellPrice = partsPrice.SellPrice;
                    reqDetails.IssueQty = item.IssueQty;
                    reqDetails.UpdateDate = DateTime.Now;
                    reqDetails.UpUserId = userId;
                }
                transferDetailRepository.Update(reqDetails);
                MobilePartStockDetailDTO stockOutItem = new MobilePartStockDetailDTO
                {
                    SWarehouseId = partsPrice.SWarehouseId,
                    MobilePartId = item.PartsId,
                    CostPrice = partsPrice.CostPrice,
                    SellPrice = partsPrice.SellPrice,
                    Quantity = item.IssueQty,
                    Remarks = reqInfo.TransferCode,
                    StockStatus = StockStatus.StockOut,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    BranchId = branchId,
                    ReferrenceNumber = reqInfo.TransferCode,
                    DescriptionId = reqInfo.DescriptionId
                };
                TransferStockOutItems.Add(stockOutItem);
            }
            if (transferInfoRepository.Save() == true)
            {
                IsSuccess = _mobilePartStockDetailBusiness.SaveMobilePartStockOut(TransferStockOutItems, userId, orgId, branchId);
            }
            return IsSuccess;
        }
    }
}
