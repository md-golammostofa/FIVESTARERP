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
   public class MobilePartStockInfoBusiness: IMobilePartStockInfoBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly MobilePartStockInfoRepository mobilePartStockInfoRepository; // repo
        public MobilePartStockInfoBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            mobilePartStockInfoRepository = new MobilePartStockInfoRepository(this._configurationDb);
        }

        public MobilePartStockInfo GetAllMobilePartStockById(long orgId, long branchId)
        {
            return mobilePartStockInfoRepository.GetOneByOrg(info => info.OrganizationId == orgId && info.BranchId == branchId);
        }

        public IEnumerable<MobilePartStockInfo> GetAllMobilePartStockByParts(long warehouseId, long partsId, long orgId, long branchId,long modelId)
        {
            return mobilePartStockInfoRepository.GetAll(info => info.SWarehouseId == warehouseId && info.MobilePartId == partsId && info.OrganizationId == orgId && info.BranchId == branchId && info.DescriptionId==modelId).ToList();
        }

        public IEnumerable<MobilePartStockInfo> GetAllMobilePartStockInfoById(long orgId, long branchId)
        {
            return mobilePartStockInfoRepository.GetAll(info => info.OrganizationId == orgId && info.BranchId == branchId).ToList();
        }

        public MobilePartStockInfo GetAllMobilePartStockInfoByInfoId(long warehouseId, long partsId, double cprice, long orgId, long branchId,long model)
        {
            return mobilePartStockInfoRepository.GetOneByOrg(info =>info.SWarehouseId==warehouseId && info.MobilePartId==partsId && info.CostPrice == cprice && info.OrganizationId == orgId && info.BranchId == branchId && info.DescriptionId==model);
        }

        public MobilePartStockInfo GetMobilePartStockInfoByModelAndMobilePartsAndCostPrice(long modelId, long mobilePartsId,double costprice, long orgId, long branchId)
        {
            var data = mobilePartStockInfoRepository.GetOneByOrg(info => info.OrganizationId == orgId && info.BranchId == branchId && info.DescriptionId == modelId && info.MobilePartId == mobilePartsId && info.CostPrice ==costprice);
            return data;
        }
        //Nishad//
        public IEnumerable<MobilePartStockInfo> GetAllMobilePartStockInfoByModelAndBranch(long orgId, long modelId, long branchId)
        {
            var data = mobilePartStockInfoRepository.GetAll(info => info.OrganizationId == orgId && info.BranchId == branchId && info.DescriptionId == modelId).ToList();
            return data;
        }

        public IEnumerable<MobilePartStockInfo> GetAllMobilePartStockInfoByOrgId(long orgId,long branchId)
        {
            var  data = mobilePartStockInfoRepository.GetAll(info => info.OrganizationId == orgId && info.BranchId == branchId).ToList();
            return data;
        }

        public MobilePartStockInfo GetAllMobilePartStockInfoBySellPrice(long warehouseId, long partsId, double sprice, long orgId, long branchId)
        {
            return mobilePartStockInfoRepository.GetOneByOrg(info => info.SWarehouseId == warehouseId && info.MobilePartId == partsId && info.CostPrice == sprice && info.OrganizationId == orgId && info.BranchId == branchId);
        }

        public IEnumerable<MobilePartStockInfoDTO> GetCurrentStock(long orgId, long branchId)
        {
            return _configurationDb.Db.Database.SqlQuery<MobilePartStockInfoDTO>(QueryForCurrentStock( orgId, branchId)).ToList();
        }
        private string QueryForCurrentStock(long orgId, long branchId)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (orgId > 0)
            {
                param += string.Format(@"and stock.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and stock.BranchId={0}", branchId);
            }

            query = string.Format(@"select MobilePartStockInfoId,d.ModelName,stock.MobilePartId,parts.MobilePartName,parts.MobilePartCode 'PartsCode',
sum(StockInQty-StockOutQty) 'Quantity' from tblMobilePartStockInfo stock
left join tblMobileParts parts on stock.MobilePartId=parts.MobilePartId
left join [Configuration].dbo.tblModelSS d on stock.DescriptionId=d.ModelId
where 1=1{0} and (StockInQty-StockOutQty) > 0
group by MobilePartStockInfoId,stock.MobilePartId,parts.MobilePartName,parts.MobilePartCode,d.ModelName
order by d.ModelName
", Utility.ParamChecker(param));
            return query;
        }

        public IEnumerable<MobilePartStockInfoDTO> GetMobilePartsStockInformations(long? warehouseId, long? modelId, long? partsId, string lessOrEq, long orgId, long branchId)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (orgId > 0)
            {
                param += string.Format(@" and stock.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@" and stock.BranchId={0}", branchId);
            }
            if (warehouseId != null && warehouseId.Value > 0)
            {
                param += string.Format(@" and stock.SWarehouseId={0}", warehouseId);
            }
            if (modelId != null && modelId.Value > 0)
            {
                param += string.Format(@" and stock.DescriptionId={0}", modelId);
            }
            if (partsId != null && partsId.Value > 0)
            {
                param += string.Format(@" and stock.MobilePartId={0}", partsId);
            }
            if (!string.IsNullOrEmpty(lessOrEq) && lessOrEq.Trim() != "")
            {
                param += string.Format(@" and (stock.StockInQty - stock.StockOutQty)<={0}", lessOrEq);
            }

            query = string.Format(@"Select stock.MobilePartStockInfoId,stock.SWarehouseId,w.ServicesWarehouseName,stock.MobilePartId
,parts.MobilePartName,parts.MobilePartCode 'PartsCode',stock.CostPrice,stock.SellPrice,stock.StockInQty
,stock.StockOutQty,stock.Remarks,stock.OrganizationId,stock.DescriptionId,de.ModelName
From tblMobilePartStockInfo stock
Left Join tblServiceWarehouses w on stock.SWarehouseId = w.SWarehouseId and stock.OrganizationId =w.OrganizationId
Left Join tblMobileParts parts on stock.MobilePartId = parts.MobilePartId and stock.OrganizationId =parts.OrganizationId
Left Join tblModelSS de  on stock.DescriptionId = de.ModelId and stock.OrganizationId =de.OrganizationId
Where 1=1{0} and (stock.StockInQty-stock.StockOutQty) > 0 order by ModelName", Utility.ParamChecker(param));

            return _configurationDb.Db.Database.SqlQuery<MobilePartStockInfoDTO>(query).ToList();
        }
        public IEnumerable<MobilePartStockInfoDTO> GetAllGoodMobilePartsAndCode(long orgId)
        {
            return this._configurationDb.Db.Database.SqlQuery<MobilePartStockInfoDTO>(
                string.Format(@"SELECT DISTINCT mpsi.MobilePartId,CAST(mp.MobilePartName AS VARCHAR(25)) +'-'+ CAST(mp.MobilePartCode AS VARCHAR(10)) 'MobilePartName'
FROM [Configuration].dbo.tblMobilePartStockInfo mpsi
Inner Join [Configuration].dbo.tblMobileParts mp On mpsi.MobilePartId = mp.MobilePartId and mpsi.OrganizationId = mp.OrganizationId
where mpsi.OrganizationId={0}", orgId)).ToList();
        }

        public IEnumerable<MobilePartStockInfo> GetAllMobilePartStockByPartsSales(long warehouseId, long partsId, long orgId, long branchId,long modelId)
        {
            return mobilePartStockInfoRepository.GetAll(info => info.SWarehouseId == warehouseId && info.MobilePartId == partsId && info.OrganizationId == orgId && info.BranchId == branchId && info.DescriptionId==modelId).ToList();
        }

        public MobilePartStockInfo GetPriceByModel(long modelId, long partsId, long orgId, long branchId)
        {
            return mobilePartStockInfoRepository.GetOneByOrg(p => p.DescriptionId == modelId && p.MobilePartId == partsId && p.OrganizationId == orgId && p.BranchId == branchId);
        }

        public IEnumerable<MobilePartStockInfoDTO> GetPartsPriceList(long orgId, long branchId, long? model, long? parts)
        {
            return _configurationDb.Db.Database.SqlQuery<MobilePartStockInfoDTO>(QueryForPriceList(orgId, branchId,model,parts)).ToList();
        }
        private string QueryForPriceList(long orgId, long branchId, long? model, long? parts)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (orgId > 0)
            {
                param += string.Format(@"and st.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and st.BranchId={0}", branchId);
            }
            if (model != null && model.Value > 0)
            {
                param += string.Format(@" and st.DescriptionId={0}", model);
            }
            if (parts != null && parts.Value > 0)
            {
                param += string.Format(@" and st.MobilePartId={0}", parts);
            }

            query = string.Format(@"Select st.DescriptionId,m.ModelName,st.MobilePartId,p.MobilePartName,p.MobilePartCode'PartsCode',st.CostPrice,st.SellPrice From tblMobilePartStockInfo st
Left Join tblModelSS m on st.DescriptionId=m.ModelId
Left Join tblMobileParts p on st.MobilePartId=p.MobilePartId
Where 1=1{0}
", Utility.ParamChecker(param));
            return query;
        }

        public IEnumerable<MobilePartStockInfo> GetPriceByModelAndParts(long modelId, long partsId, long orgId, long branchId)
        {
            return mobilePartStockInfoRepository.GetAll(s => s.DescriptionId == modelId && s.MobilePartId == partsId && s.OrganizationId == orgId && s.BranchId == branchId).ToList();
        }
    }
}
