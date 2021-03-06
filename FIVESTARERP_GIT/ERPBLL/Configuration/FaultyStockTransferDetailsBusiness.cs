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
    public class FaultyStockTransferDetailsBusiness: IFaultyStockTransferDetailsBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb;
        private readonly FaultyStockTransferDetailsRepository _faultyStockTransferDetailsRepository;//repo
        private readonly FaultyStockTransferInfoRepository _faultyStockTransferInfoRepository;//repo
        private readonly IFaultyStockTransferInfoBusiness _faultyStockTransferInfoBusiness;
        private readonly IFaultyStockDetailBusiness _faultyStockDetailBusiness;
        public FaultyStockTransferDetailsBusiness(IConfigurationUnitOfWork configurationDb, IFaultyStockTransferInfoBusiness faultyStockTransferInfoBusiness, IFaultyStockDetailBusiness faultyStockDetailBusiness)
        {
            this._configurationDb = configurationDb;
            _faultyStockTransferDetailsRepository = new FaultyStockTransferDetailsRepository(this._configurationDb);
            _faultyStockTransferInfoRepository = new FaultyStockTransferInfoRepository(this._configurationDb);
            this._faultyStockTransferInfoBusiness = faultyStockTransferInfoBusiness;
            this._faultyStockDetailBusiness = faultyStockDetailBusiness;
        }

        public IEnumerable<FaultyStockTransferDetails> GetAllDetails(long orgId, long branchId)
        {
            return _faultyStockTransferDetailsRepository.GetAll(d => d.OrganizationId == orgId && d.BranchId == branchId).ToList();
        }

        public IEnumerable<FaultyStockTransferDetails> GetAllDetailsByInfoId(long infoId, long orgId, long branchId)
        {
            return _faultyStockTransferDetailsRepository.GetAll(d =>d.FSTInfoId==infoId && d.OrganizationId == orgId && d.BranchId == branchId).ToList();
        }

        public IEnumerable<FaultyStockTransferDetails> GetAllReceiveDetailsByInfoId(long infoId, long orgId)
        {
            return _faultyStockTransferDetailsRepository.GetAll(d => d.FSTInfoId == infoId && d.OrganizationId == orgId).ToList();
        }

        public FaultyStockTransferDetails GetOneByOneDetails(long detailId, long orgId, long branchId)
        {
            return _faultyStockTransferDetailsRepository.GetOneByOrg(d => d.FSTDetailsId == detailId && d.OrganizationId == orgId && d.BranchId == branchId);
        }

        public FaultyStockTransferDetails GetOneDetailsByInfoId(long infoId, long orgId, long branchId)
        {
            return _faultyStockTransferDetailsRepository.GetOneByOrg(d => d.FSTInfoId == infoId && d.OrganizationId == orgId && d.BranchId == branchId);
        }

        public bool SaveFaultyStockTransfer(FaultyStockTransferInfoDTO dto, long userId, long orgId, long branchId)
        {
            bool IsSuccess = false;
            var transferCode = ("FST-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
            List<FaultyStockDetailDTO> faultyStock = new List<FaultyStockDetailDTO>();
            List<FaultyStockTransferDetails> details = new List<FaultyStockTransferDetails>();

            FaultyStockTransferInfo info = new FaultyStockTransferInfo();
            info.TransferCode = transferCode;
            info.StateStatus = "Send";
            info.BranchTo = dto.BranchTo;
            info.OrganizationId = orgId;
            info.BranchFrom = branchId;
            info.BranchId = branchId;
            info.EUserId = userId;
            info.EntryDate = DateTime.Now;

            foreach (var item in dto.faultyStockTransferDetails)
            {
                FaultyStockTransferDetails faulty = new FaultyStockTransferDetails
                {
                    ModelId = item.ModelId,
                    PartsId = item.PartsId,
                    CostPrice = 0,
                    SellPrice = 0,
                    Quantity = item.Quantity,
                    EUserId = userId,
                    OrganizationId = orgId,
                    BranchId = branchId,
                    EntryDate = DateTime.Now,
                };
                details.Add(faulty);
                FaultyStockDetailDTO faultydto = new FaultyStockDetailDTO
                {
                    DescriptionId=item.ModelId,
                    PartsId=item.PartsId,
                    CostPrice=item.CostPrice,
                    SellPrice=item.SellPrice,
                    Quantity=item.Quantity,
                    StateStatus="Stock-Out",
                    EUserId=userId,
                    OrganizationId = orgId,
                    BranchId = branchId,
                    EntryDate = DateTime.Now,
                };
                faultyStock.Add(faultydto);
            }
            info.faultyStockTransferDetails = details;
            _faultyStockTransferInfoRepository.Insert(info);
            if (_faultyStockTransferInfoRepository.Save())
            {
                IsSuccess = _faultyStockDetailBusiness.SaveFaultyStockOut(faultyStock, userId, orgId, branchId);
            }
            return IsSuccess;
        }

        public bool SaveFaultyStockReceive(long transferId, long userId, long orgId, long branchId)
        {
            bool IsSuccess = false;
            if (transferId > 0)
            {
                var infoInDb = _faultyStockTransferInfoBusiness.GetOneByOneInfoByStatus(transferId, orgId);
                if(infoInDb != null)
                {
                    infoInDb.StateStatus = "Received";
                    infoInDb.UpdateDate = DateTime.Now;
                    infoInDb.UpUserId = userId;
                    _faultyStockTransferInfoRepository.Update(infoInDb);
                }
                if (_faultyStockTransferInfoRepository.Save())
                {
                    List<FaultyStockDetailDTO> faultyStock = new List<FaultyStockDetailDTO>();
                    var details = GetAllReceiveDetailsByInfoId(transferId, orgId);
                    if (details.Count() > 0)
                    {
                        foreach(var item in details)
                        {
                            FaultyStockDetailDTO faultydto = new FaultyStockDetailDTO
                            {
                                DescriptionId = item.ModelId,
                                PartsId = item.PartsId,
                                CostPrice = item.CostPrice,
                                SellPrice = item.SellPrice,
                                Quantity = item.Quantity,
                                StateStatus = "Stock-In",
                                EUserId = userId,
                                OrganizationId = orgId,
                                BranchId = branchId,
                                EntryDate = DateTime.Now,
                            };
                            faultyStock.Add(faultydto);
                        }
                    }
                    IsSuccess = _faultyStockDetailBusiness.SaveFaultyStockIn(faultyStock, userId, orgId, branchId);
                }
            }
            return IsSuccess;
        }
    }
}
