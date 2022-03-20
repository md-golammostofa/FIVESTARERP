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
    public class PartsTransferHToCInfoBusiness: IPartsTransferHToCInfoBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly PartsTransferHToCInfoRepository partsTransferHToCInfoRepository; // repo
        private readonly MobilePartStockDetailRepository mobilePartStockDetailRepository;
        private readonly MobilePartStockInfoRepository mobilePartStockInfoRepository;
        private readonly IMobilePartStockDetailBusiness _mobilePartStockDetailBusiness;
        private readonly IMobilePartStockInfoBusiness _mobilePartStockInfoBusiness;
        private readonly IServicesWarehouseBusiness _servicesWarehouseBusiness;
        private readonly IPartsTransferHToCDetailsBusiness _partsTransferHToCDetailsBusiness;
        public PartsTransferHToCInfoBusiness(IConfigurationUnitOfWork configurationDb, IMobilePartStockDetailBusiness mobilePartStockDetailBusiness, IMobilePartStockInfoBusiness mobilePartStockInfoBusiness, IServicesWarehouseBusiness servicesWarehouseBusiness, IPartsTransferHToCDetailsBusiness partsTransferHToCDetailsBusiness)
        {
            this._configurationDb = configurationDb;
            partsTransferHToCInfoRepository = new PartsTransferHToCInfoRepository(this._configurationDb);
            mobilePartStockDetailRepository = new MobilePartStockDetailRepository(this._configurationDb);
            mobilePartStockInfoRepository = new MobilePartStockInfoRepository(this._configurationDb);
            this._mobilePartStockDetailBusiness = mobilePartStockDetailBusiness;
            this._mobilePartStockInfoBusiness = mobilePartStockInfoBusiness;
            this._servicesWarehouseBusiness = servicesWarehouseBusiness;
            this._partsTransferHToCDetailsBusiness = partsTransferHToCDetailsBusiness;
        }

        public bool SavePartsTransferHeadToCare(PartsTransferHToCInfoDTO dto, List<PartsTransferHToCDetailsDTO> details, long orgId, long branchId, long userId)
        {
            var IsSuccess = false;
            Random random = new Random();
            var ran = random.Next().ToString();
            ran = ran.Substring(0, 4);
            var code = ("HTC-" + ran + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
            List<PartsTransferHToCDetails> hToCDetailsList = new List<PartsTransferHToCDetails>();
            if (details.Count() > 0)
            {

                PartsTransferHToCInfo hToCInfo = new PartsTransferHToCInfo();
                hToCInfo.BranchToId = dto.BranchToId;
                hToCInfo.StateStatus = "Send";
                hToCInfo.TransferCode = code;
                hToCInfo.BranchFromId = branchId;
                hToCInfo.BranchId = branchId;
                hToCInfo.EntryDate = DateTime.Now;
                hToCInfo.OrganizationId = orgId;
                hToCInfo.EUserId = userId;

                foreach(var item in details)
                {
                    var stock = _mobilePartStockInfoBusiness.GetPriceByModelAndParts(item.ModelId, item.PartsId, orgId, branchId);
                    var stockCheck = stock.Sum(s => s.StockInQty - s.StockOutQty);
                    if(stockCheck > 0 &&(stockCheck > item.Quantity || stockCheck == item.Quantity))
                    {
                        PartsTransferHToCDetails hToCDetails = new PartsTransferHToCDetails
                        {
                            ModelId = item.ModelId,
                            PartsId = item.PartsId,
                            CostPrice = item.CostPrice,
                            SellPrice = item.SellPrice,
                            Quantity = item.Quantity,
                            EntryDate = DateTime.Now,
                            EUserId = userId,
                            OrganizationId = orgId,
                            BranchId = branchId,
                            RefCode = code,
                            Remarks = "Stock-Transfer To Branch",
                        };
                        hToCDetailsList.Add(hToCDetails);
                    }
                }
                hToCInfo.partsTransferHToCDetails = hToCDetailsList;
                partsTransferHToCInfoRepository.Insert(hToCInfo);
            }
            return partsTransferHToCInfoRepository.Save();
        }
        public bool SaveMobilePartStockOut(PartsTransferHToCInfoDTO dto, long orgId, long branchId, long userId)
        {
            bool IsSuccess = false;
            var warehouse = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(orgId, branchId).FirstOrDefault();
            List<MobilePartStockDetail> stockDetails = new List<MobilePartStockDetail>();
            List<PartsTransferHToCDetailsDTO> detailsDTO = new List<PartsTransferHToCDetailsDTO>();
            foreach (var item in dto.partsTransferHToCDetails)
            {
                var reqQty = item.Quantity;
                var partsInStock = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByModelAndBranch(orgId, item.ModelId, branchId).Where(i => i.MobilePartId == item.PartsId && (i.StockInQty - i.StockOutQty) > 0).OrderBy(i => i.MobilePartStockInfoId).ToList();

                if (partsInStock.Count() > 0)
                {
                    int remainQty = reqQty;
                    foreach (var stock in partsInStock)
                    {

                        var totalStockqty = (stock.StockInQty - stock.StockOutQty); // total stock
                        var stockOutQty = 0;
                        if (totalStockqty <= remainQty)
                        {
                            stock.StockOutQty += totalStockqty;
                            stockOutQty = totalStockqty.Value;
                            remainQty -= totalStockqty.Value;
                        }
                        else
                        {
                            stockOutQty = remainQty;
                            stock.StockOutQty += remainQty;
                            remainQty = 0;
                        }


                        MobilePartStockDetail stockDetail = new MobilePartStockDetail()
                        {
                            DescriptionId = item.ModelId,
                            SWarehouseId = warehouse.SWarehouseId,
                            MobilePartId = item.PartsId,
                            CostPrice = stock.CostPrice,
                            SellPrice = stock.SellPrice,
                            Quantity = stockOutQty,
                            Remarks = item.Remarks,
                            OrganizationId = orgId,
                            BranchId = branchId,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            StockStatus = StockStatus.StockOut,
                        };
                        stockDetails.Add(stockDetail);

                        PartsTransferHToCDetailsDTO parrtsDTO = new PartsTransferHToCDetailsDTO
                        {
                            ModelId=item.ModelId,
                            PartsId=item.PartsId,
                            CostPrice=stock.CostPrice,
                            SellPrice=stock.SellPrice,
                            Quantity= stockOutQty,
                        };
                        detailsDTO.Add(parrtsDTO);
                        mobilePartStockInfoRepository.Update(stock);
                        if (remainQty == 0)
                        {
                            break;
                        }
                    }
                }
            }
            mobilePartStockDetailRepository.InsertAll(stockDetails);
            if (mobilePartStockDetailRepository.Save() == true)
            {
                IsSuccess = SavePartsTransferHeadToCare(dto, detailsDTO, orgId, branchId, userId);
            }
            return IsSuccess;
        }

        public IEnumerable<PartsTransferHToCInfoDTO> GetAllPartsTransferData(long orgId, long branchId)
        {
            return this._configurationDb.Db.Database.SqlQuery<PartsTransferHToCInfoDTO>(string.Format(@"Select pt.InfoId,pt.TransferCode,pt.StateStatus,pt.EntryDate,b.BranchName'BranchToName' From tblPartsTransferHToCInfo pt
Left Join [ControlPanel].dbo.tblBranch b on pt.BranchToId=b.BranchId
Where pt.OrganizationId={0} and pt.BranchId={1}", orgId, branchId)).ToList();
        }

        public IEnumerable<PartsTransferHToCInfoDTO> GetAllPartsReceiveData(long orgId, long branchId, string fromDate, string toDate)
        {
            return this._configurationDb.Db.Database.SqlQuery<PartsTransferHToCInfoDTO>(QueryForGetAllPartsReceiveData(orgId,branchId,fromDate,toDate)).ToList();
        }
        private string QueryForGetAllPartsReceiveData(long orgId,long branchId,string fromDate,string toDate)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (orgId > 0)
            {
                param += string.Format(@"and pt.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and pt.BranchToId={0}", branchId);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(pt.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(pt.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(pt.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select pt.InfoId,pt.TransferCode,pt.StateStatus,pt.EntryDate,b.BranchName'BranchFromName' From tblPartsTransferHToCInfo pt
Left Join [ControlPanel].dbo.tblBranch b on pt.BranchId=b.BranchId
Where 1=1{0}", Utility.ParamChecker(param));
            return query;
        }

        public PartsTransferHToCInfo GetInfoByInfoId(long infoId, long orgId)
        {
            return partsTransferHToCInfoRepository.GetOneByOrg(i => i.InfoId == infoId && i.OrganizationId == orgId);
        }

        public bool SaveTransferItemReceive(long infoId, long userId, long orgId, long branchId)
        {
            var warehouse = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(orgId, branchId).FirstOrDefault();
            bool IsSuccess = false;
            if (infoId > 0)
            {
                var infoItem = GetInfoByInfoId(infoId, orgId);
                if(infoItem != null)
                {
                    infoItem.StateStatus = "Receive";
                    infoItem.UpUserId = userId;
                    infoItem.UpdateDate = DateTime.Now;
                    partsTransferHToCInfoRepository.Update(infoItem);
                }
                if (partsTransferHToCInfoRepository.Save() == true)
                {
                    List<MobilePartStockDetailDTO> stock = new List<MobilePartStockDetailDTO>();
                    var details = _partsTransferHToCDetailsBusiness.GetAllDetailsByInfoId(infoId, orgId);
                    if (details.Count() > 0)
                    {
                        foreach(var item in details)
                        {
                            MobilePartStockDetailDTO stockDetails = new MobilePartStockDetailDTO
                            {
                                DescriptionId=item.ModelId,
                                MobilePartId=item.PartsId,
                                CostPrice=item.CostPrice,
                                SellPrice=item.SellPrice,
                                Quantity=item.Quantity,
                                SWarehouseId= warehouse.SWarehouseId,
                                StockStatus ="Stock-In",
                                Remarks="Stock-In By HTC",
                                ReferrenceNumber=infoItem.TransferCode,
                            };
                            stock.Add(stockDetails);
                        }
                    }
                    IsSuccess = _mobilePartStockDetailBusiness.SaveMobilePartStockInByHTC(stock, userId, orgId, branchId);
                }
            }
            return IsSuccess;
        }
    }
}
