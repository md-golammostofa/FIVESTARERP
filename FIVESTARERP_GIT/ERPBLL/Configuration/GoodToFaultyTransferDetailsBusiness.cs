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
    public class GoodToFaultyTransferDetailsBusiness: IGoodToFaultyTransferDetailsBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly GoodToFaultyTransferDetailsRepository goodToFaultyTransferDetailsRepository; // repo
        private readonly GoodToFaultyTransferInfoRepository goodToFaultyTransferInfoRepository; // repo
        private readonly IMobilePartStockDetailBusiness _mobilePartStockDetailBusiness;
        private readonly IFaultyStockDetailBusiness _faultyStockDetailBusiness;
        public GoodToFaultyTransferDetailsBusiness(IConfigurationUnitOfWork configurationDb, IMobilePartStockDetailBusiness mobilePartStockDetailBusiness, IFaultyStockDetailBusiness faultyStockDetailBusiness)
        {
            this._configurationDb = configurationDb;
            goodToFaultyTransferDetailsRepository = new GoodToFaultyTransferDetailsRepository(this._configurationDb);
            goodToFaultyTransferInfoRepository = new GoodToFaultyTransferInfoRepository(this._configurationDb);
            this._mobilePartStockDetailBusiness = mobilePartStockDetailBusiness;
            this._faultyStockDetailBusiness = faultyStockDetailBusiness;
        }

        public IEnumerable<GoodToFaultyTransferDetailsDTO> GetGoodToFaultyTransferDetailsList(long orgId, long branchId)
        {
           return this._configurationDb.Db.Database.SqlQuery<GoodToFaultyTransferDetailsDTO>(QueryForGoodToFaultyTransferDetails(orgId, branchId)).ToList();
        }
        private string QueryForGoodToFaultyTransferDetails(long orgId,long branchId)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (orgId > 0)
            {
                param += string.Format(@"and d.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and d.BranchId={0}", branchId);
            }
            query = string.Format(@"Select d.GTFTInfoId,d.GTFTDetailsId,d.ModelId,m.ModelName,
d.PartsId,p.MobilePartName'PartsName',p.MobilePartCode'PartsCode',d.Quantity 
From tblGoodToFaultyTransferDetails d
Left Join tblModelSS m on d.ModelId=m.ModelId
Left Join tblMobileParts p on d.PartsId=p.MobilePartId
Where 1=1{0}
Order By d.EntryDate desc", Utility.ParamChecker(param));
            return query;
        }

        public bool SaveStockOutAndFaultyStockIn(GoodToFaultyTransferInfoDTO dto, long userId, long orgId, long branchId)
        {
            bool IsSuccess = false;
            Random random = new Random();
            var ran = random.Next().ToString();
            ran = ran.Substring(0, 4);
            var code = ("GTF-" + ran + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
            List<GoodToFaultyTransferDetails> transDetailList = new List<GoodToFaultyTransferDetails>();
            List<MobilePartStockDetailDTO> goodStockList = new List<MobilePartStockDetailDTO>();
            List<FaultyStockDetailDTO> faultyStockList = new List<FaultyStockDetailDTO>();
            if (dto.goodToFaultyTransferDetails.Count() > 0)
            {
                GoodToFaultyTransferInfo transInfo = new GoodToFaultyTransferInfo();
                transInfo.TransferCode = code;
                transInfo.TransferStatus = "Transfer To Faulty";
                transInfo.OrganizationId = orgId;
                transInfo.BranchId = branchId;
                transInfo.EntryDate = DateTime.Now;
                transInfo.EUserId = userId;

                foreach(var item in dto.goodToFaultyTransferDetails)
                {
                    GoodToFaultyTransferDetails goodToFaulty = new GoodToFaultyTransferDetails
                    {
                        ModelId = item.ModelId,
                        PartsId = item.PartsId,
                        Quantity = item.Quantity,
                        CostPrice = 0,
                        SellPrice=0,
                        OrganizationId=orgId,
                        BranchId=branchId,
                        EUserId=userId,
                        EntryDate=DateTime.Now,
                    };
                    transDetailList.Add(goodToFaulty);

                    MobilePartStockDetailDTO goodStock = new MobilePartStockDetailDTO
                    {
                        DescriptionId=item.ModelId,
                        MobilePartId=item.PartsId,
                        SWarehouseId=dto.WarehouseId,
                        CostPrice=0,
                        SellPrice=0,
                        Quantity=item.Quantity,
                    };
                    goodStockList.Add(goodStock);

                    FaultyStockDetailDTO faultyStock = new FaultyStockDetailDTO
                    {
                        DescriptionId = item.ModelId,
                        PartsId = item.PartsId,
                        SWarehouseId = dto.WarehouseId,
                        CostPrice = 0,
                        SellPrice = 0,
                        Quantity = item.Quantity,
                    };
                    faultyStockList.Add(faultyStock);
                }
                transInfo.goodToFaultyTransferDetails = transDetailList;
                goodToFaultyTransferInfoRepository.Insert(transInfo);
            }
            if (goodToFaultyTransferInfoRepository.Save() == true)
            {
                IsSuccess = _mobilePartStockDetailBusiness.SaveMobilePartStockOutByGoodToFaulty(goodStockList, orgId, branchId, userId);
                if (IsSuccess == true)
                {
                    return _faultyStockDetailBusiness.SaveFaultyStockInByGoodToFaulty(faultyStockList, userId, orgId, branchId);
                }
            }
            return IsSuccess;
        }
    }
}
