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
    public class FaultyStockRepairedInfoBusiness: IFaultyStockRepairedInfoBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly FaultyStockRepairedInfoRepository _faultyStockRepairedInfoRepository; // repo
        private readonly IFaultyStockInfoBusiness _faultyStockInfoBusiness;
        private readonly IFaultyStockDetailBusiness _faultyStockDetailBusiness;
        private readonly IServicesWarehouseBusiness _servicesWarehouseBusiness;
        private readonly IFaultyStockRepairedDetailsBusiness _faultyStockRepairedDetailsBusiness;
        private readonly FaultyStockRepairedDetailsRepository _faultyStockRepairedDetailsRepository; // repo
        private readonly IMobilePartStockDetailBusiness _mobilePartStockDetailBusiness;
        private readonly IScrapStockDetailBusiness _scrapStockDetailBusiness;
        private readonly IMobilePartStockInfoBusiness _mobilePartStockInfoBusiness;
        public FaultyStockRepairedInfoBusiness(IConfigurationUnitOfWork configurationDb, IFaultyStockInfoBusiness faultyStockInfoBusiness, IFaultyStockDetailBusiness faultyStockDetailBusiness, IServicesWarehouseBusiness servicesWarehouseBusiness, IFaultyStockRepairedDetailsBusiness faultyStockRepairedDetailsBusiness, IMobilePartStockDetailBusiness mobilePartStockDetailBusiness, IScrapStockDetailBusiness scrapStockDetailBusiness, IMobilePartStockInfoBusiness mobilePartStockInfoBusiness)
        {
            this._configurationDb = configurationDb;
            _faultyStockRepairedInfoRepository = new FaultyStockRepairedInfoRepository(this._configurationDb);
            _faultyStockRepairedDetailsRepository = new FaultyStockRepairedDetailsRepository(this._configurationDb);
            this._faultyStockInfoBusiness = faultyStockInfoBusiness;
            this._faultyStockDetailBusiness = faultyStockDetailBusiness;
            this._servicesWarehouseBusiness = servicesWarehouseBusiness;
            this._faultyStockRepairedDetailsBusiness = faultyStockRepairedDetailsBusiness;
            this._mobilePartStockDetailBusiness = mobilePartStockDetailBusiness;
            this._scrapStockDetailBusiness = scrapStockDetailBusiness;
            this._mobilePartStockInfoBusiness = mobilePartStockInfoBusiness;


        }

        public IEnumerable<FaultyStockRepairedInfoDTO> GetAllRepairedList(long orgId, long branchId, long? tsId, string fromDate, string toDate)
        {
            var data = this._configurationDb.Db.Database.SqlQuery<FaultyStockRepairedInfoDTO>(QueryForAllReapired(orgId,branchId,tsId,fromDate,toDate)).ToList();
            return data;
        }
        private string QueryForAllReapired(long orgId, long branchId,long? tsId,string fromDate,string toDate)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (orgId > 0)
            {
                param += string.Format(@"and fs.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and fs.BranchId={0}", branchId);
            }
            if (tsId != null && tsId > 0)
            {
                param += string.Format(@"and fs.TSId ={0}", tsId);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(fs.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(fs.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(fs.EntryDate as date)='{0}'", tDate);
            }
            query = string.Format(@"Select fs.FSRInfoId,fs.Code,fs.TSId,fs.StateStatus,u.UserName'TSName',fs.AssignDate,fs.RepairedDate 
From tblFaultyStockRepairedInfo fs
Left Join [ControlPanel].dbo.tblApplicationUsers u on fs.TSId=u.UserId
Where 1=1{0} Order By fs.EntryDate desc", Utility.ParamChecker(param));
            return query;
        }

        public IEnumerable<FaultyStockRepairedInfoDTO> GetAllRepairedListForTS(long tsId,long orgId, long branchId)
        {
            var data = this._configurationDb.Db.Database.SqlQuery<FaultyStockRepairedInfoDTO>(string.Format(@"Select fs.FSRInfoId,fs.TSId,fs.Code,fs.StateStatus,u.UserName'TSName',fs.AssignDate,fs.RepairedDate 
From tblFaultyStockRepairedInfo fs
Left Join [ControlPanel].dbo.tblApplicationUsers u on fs.TSId=u.UserId
Where fs.TSId={0} and fs.OrganizationId={1} and fs.BranchId={2} Order By fs.EntryDate desc",tsId, orgId, branchId)).ToList();
            return data;
        }

        public FaultyStockRepairedInfo GetRepairedInfoById(long infoId, long orgId, long branchId)
        {
            return _faultyStockRepairedInfoRepository.GetOneByOrg(i => i.FSRInfoId == infoId && i.OrganizationId == orgId && i.BranchId == branchId);
        }

        public FaultyStockRepairedInfoDTO GetRepairedInfoDataByInfoId(long infoId, long orgId, long branchId)
        {
            var data = this._configurationDb.Db.Database.SqlQuery<FaultyStockRepairedInfoDTO>(string.Format(@"Select fs.FSRInfoId,fs.TSId,fs.Code,fs.StateStatus,u.UserName'TSName',fs.AssignDate,fs.RepairedDate 
From tblFaultyStockRepairedInfo fs
Left Join [ControlPanel].dbo.tblApplicationUsers u on fs.TSId=u.UserId
Where fs.FSRInfoId={0} and fs.OrganizationId={1} and fs.BranchId={2}", infoId, orgId, branchId)).FirstOrDefault();
            return data;
        }

        public bool SaveFaultyStockRepairedAndStockOut(FaultyStockRepairedInfoDTO dto, long userId, long orgId, long branchId)
        {
            int ftyQty = 0;
            bool IsSuccess = false;
            Random random = new Random();
            var ran = random.Next().ToString();
            ran = ran.Substring(0, 4);
            var code = ("FSR-" + ran + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
            var warehouse = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(orgId, branchId).FirstOrDefault();

            FaultyStockRepairedInfo repairedInfo = new FaultyStockRepairedInfo();
            List<FaultyStockRepairedDetails> faultyDetailList = new List<FaultyStockRepairedDetails>();
            List<FaultyStockDetailDTO> faultyStock = new List<FaultyStockDetailDTO>();

            if (dto.faultyStockRepairedDetails.Count() > 0)
            {
                foreach(var item in dto.faultyStockRepairedDetails)
                {
                    var availableQty = _faultyStockInfoBusiness.GetAllFaultyByModelAndParts(item.ModelId, item.PartsId, orgId, branchId);
                    ftyQty = availableQty.Sum(s => (s.StockInQty - s.StockOutQty));
                    if(ftyQty > item.AssignQty || ftyQty == item.AssignQty)
                    {
                        FaultyStockRepairedDetails details = new FaultyStockRepairedDetails
                        {
                            ModelId = item.ModelId,
                            PartsId = item.PartsId,
                            TSId = dto.TSId,
                            RefCode = code,
                            AssignQty = item.AssignQty,
                            EntryDate = DateTime.Now,
                            RepairedQty = 0,
                            ScrapedQty = 0,
                            OrganizationId = orgId,
                            BranchId = branchId,
                            EUserId = userId,
                        };
                        faultyDetailList.Add(details);
                        FaultyStockDetailDTO stockDetail = new FaultyStockDetailDTO
                        {
                            DescriptionId=item.ModelId,
                            PartsId=item.PartsId,
                            JobOrderId=0,
                            CostPrice=0,
                            SellPrice=0,
                            SWarehouseId= warehouse.SWarehouseId,
                            BranchId=branchId,
                            OrganizationId=orgId,
                            EUserId=userId,
                            StateStatus="Stock-Out",
                            Quantity=item.AssignQty,
                            TSId=dto.TSId,
                        };
                        faultyStock.Add(stockDetail);
                    }
                }
                if (faultyDetailList.Count() > 0)
                {
                    repairedInfo.Code = code;
                    repairedInfo.TSId = dto.TSId;
                    repairedInfo.StateStatus = "TS-Assigned";
                    repairedInfo.EUserId = userId;
                    repairedInfo.AssignDate = DateTime.Now;
                    repairedInfo.OrganizationId = orgId;
                    repairedInfo.BranchId = branchId;
                    repairedInfo.EntryDate = DateTime.Now;
                    repairedInfo.faultyStockRepairedDetails = faultyDetailList;
                }
                _faultyStockRepairedInfoRepository.Insert(repairedInfo);
            }
            if (_faultyStockRepairedInfoRepository.Save() == true)
            {
                IsSuccess = _faultyStockDetailBusiness.SaveFaultyStockOutByTSRepaired(faultyStock, userId, orgId, branchId);
            }
            return IsSuccess;
        }

        public bool UpdateInfoStatusAndRepairedDetails(FaultyStockRepairedInfoDTO dto, long userId, long orgId, long branchId)
        {
            bool IsSuccess = false;
            foreach(var item2 in dto.faultyStockRepairedDetails)
            {
                if(item2.AssignQty == (item2.RepairedQty + item2.ScrapedQty))
                {
                    IsSuccess = true;
                }
                else
                {
                    return IsSuccess = false;
                }
            }

            if (dto.faultyStockRepairedDetails.Count() > 0)
            {
                foreach(var item in dto.faultyStockRepairedDetails)
                {
                    if (item.AssignQty == (item.RepairedQty + item.ScrapedQty))
                    {
                        var details = _faultyStockRepairedDetailsBusiness.GetDetailsByDetailsId(item.FSRDetailsId, orgId, branchId);
                        if (details != null)
                        {
                            details.RepairedQty = item.RepairedQty;
                            details.ScrapedQty = item.ScrapedQty;
                            details.RepairedDate = DateTime.Now;
                            details.ScrapedDate = DateTime.Now;
                            details.UpdateDate = DateTime.Now;
                            details.UpUserId = userId;
                        }
                        _faultyStockRepairedDetailsRepository.Update(details);
                        _faultyStockRepairedDetailsRepository.Save();
                    }
                }
                var info = GetRepairedInfoById(dto.FSRInfoId, orgId, branchId);
                if(info != null)
                {
                    info.StateStatus = dto.StateStatus;
                    info.RepairedDate = DateTime.Now;
                    info.ReceiveUserId = userId;
                    info.UpdateDate = DateTime.Now;
                    info.UpUserId = userId;
                }
                _faultyStockRepairedInfoRepository.Update(info);
            }
            return _faultyStockRepairedInfoRepository.Save();
        }

        public bool UpdateInfoStatusAndRepairedStockIn(FaultyStockRepairedInfoDTO dto, long userId, long orgId, long branchId)
        {
            bool IsSuccess = false;
            List<MobilePartStockDetailDTO> goodStockList = new List<MobilePartStockDetailDTO>();
            List<ScrapStockDetailDTO> scrapedList = new List<ScrapStockDetailDTO>();
            var warehouse = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(orgId, branchId).FirstOrDefault();
            if (dto.faultyStockRepairedDetails.Count() > 0)
            {
                foreach(var item in dto.faultyStockRepairedDetails)
                {
                    var partsPrice = _mobilePartStockInfoBusiness.GetPriceByModelAndParts(item.ModelId, item.PartsId,orgId,branchId);
                    var costs = partsPrice.LastOrDefault().CostPrice;
                    var sell = partsPrice.LastOrDefault().SellPrice;
                    MobilePartStockDetailDTO gStock = new MobilePartStockDetailDTO
                    {
                        DescriptionId = item.ModelId,
                        MobilePartId = item.PartsId,
                        CostPrice = costs,
                        SellPrice = sell,
                        Quantity = item.RepairedQty,
                        StockStatus = "Stock-In",
                        SWarehouseId = warehouse.SWarehouseId,
                        Remarks = "Faulty Repaired"
                    };
                    goodStockList.Add(gStock);

                    ScrapStockDetailDTO scStock = new ScrapStockDetailDTO
                    {
                        DescriptionId = item.ModelId,
                        PartsId = item.PartsId,
                        CostPrice = 0,
                        SellPrice = 0,
                        Quantity= item.ScrapedQty,
                        StockStatus = "Stock-In",
                    };
                    scrapedList.Add(scStock);
                }
                var info = GetRepairedInfoById(dto.faultyStockRepairedDetails.FirstOrDefault().FSRInfoId, orgId, branchId);
                if(info != null)
                {
                    info.StateStatus = "Receive-Done";
                    info.ReceiveUserId = userId;
                    info.ReceiveDate = DateTime.Now;
                    info.UpUserId = userId;
                    info.UpdateDate = DateTime.Now;
                }
                _faultyStockRepairedInfoRepository.Update(info);
                if (_faultyStockRepairedInfoRepository.Save() == true)
                {
                    IsSuccess = _mobilePartStockDetailBusiness.SaveMobilePartStockInByRepaired(goodStockList, userId, orgId, branchId);
                    if (IsSuccess == true)
                    {
                        IsSuccess = _scrapStockDetailBusiness.SaveScrapStockIn(scrapedList, userId, orgId, branchId);
                    }
                }
            }
            return IsSuccess;
        }
    }
}
