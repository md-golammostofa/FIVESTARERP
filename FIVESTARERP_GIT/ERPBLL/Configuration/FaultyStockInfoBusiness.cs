﻿using ERPBLL.Common;
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
    public class FaultyStockInfoBusiness : IFaultyStockInfoBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb;
        private readonly FaultyStockInfoRepository _faultyStockInfoRepository;
        public FaultyStockInfoBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            _faultyStockInfoRepository = new FaultyStockInfoRepository(this._configurationDb); 
        }

        public IEnumerable<FaultyStockInfo> GetAllFaultyStockInfoByOrgId(long orgId, long branchId)
        {
            return _faultyStockInfoRepository.GetAll(s => s.OrganizationId == orgId && s.BranchId == branchId);
        }
        public FaultyStockInfo GetAllFaultyStockInfoByModelAndPartsIdAndCostPrice(long modelId, long partsId, double cprice, long orgId, long branchId)
        {
            return _faultyStockInfoRepository.GetOneByOrg(s => s.DescriptionId == modelId && s.PartsId == partsId && s.CostPrice == cprice && s.OrganizationId == orgId && s.BranchId == branchId);
        }
        public IEnumerable<FaultyStockInfoDTO> GetFaultyStockInfoByQuery(long? warehouseId, long? modelId, long? partsId, string lessOrEq, long orgId, long branchId)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (warehouseId != null && warehouseId.Value > 0)
            {
                param += string.Format(@" and fs.SWarehouseId={0}", warehouseId);
            }
            if (modelId != null && modelId.Value > 0)
            {
                param += string.Format(@" and fs.DescriptionId={0}", modelId);
            }
            if (partsId != null && partsId.Value > 0)
            {
                param += string.Format(@" and fs.PartsId={0}", partsId);
            }
            if (!string.IsNullOrEmpty(lessOrEq) && lessOrEq.Trim() != "")
            {
                param += string.Format(@" and (fs.StockInQty - fs.StockOutQty)<={0}", lessOrEq);
            }

            query = string.Format(@"Select fs.FaultyStockInfoId,fs.SWarehouseId,w.ServicesWarehouseName,fs.PartsId
,parts.MobilePartName,parts.MobilePartCode 'PartsCode',fs.CostPrice,fs.SellPrice,fs.StockInQty,fs.StockOutQty,fs.Remarks,fs.OrganizationId,fs.DescriptionId,de.ModelName 
From tblFaultyStockInfo fs
Left Join tblServiceWarehouses w on fs.SWarehouseId = w.SWarehouseId and fs.OrganizationId =w.OrganizationId
Left Join tblMobileParts parts on fs.PartsId = parts.MobilePartId and fs.OrganizationId =parts.OrganizationId
Left Join [Configuration].dbo.tblModelSS de  on fs.DescriptionId = de.ModelId and fs.OrganizationId =de.OrganizationId
Where 1=1 and fs.OrganizationId={0} and fs.BranchId= {1} {2}", orgId, branchId, Utility.ParamChecker(param));

            return _configurationDb.Db.Database.SqlQuery<FaultyStockInfoDTO>(query).ToList();
        }

        public IEnumerable<FaultyStockInfoDTO> GetAllFaultyMobilePartsAndCode(long orgId)
        {
            return this._configurationDb.Db.Database.SqlQuery<FaultyStockInfoDTO>(
                string.Format(@"SELECT DISTINCT fsi.PartsId,CAST(mp.MobilePartName AS VARCHAR(25)) +'-'+ CAST(mp.MobilePartCode AS VARCHAR(10)) 'MobilePartName'
FROM [Configuration].dbo.tblFaultyStockInfo fsi
Inner Join [Configuration].dbo.tblMobileParts mp On fsi.PartsId = mp.MobilePartId and fsi.OrganizationId = mp.OrganizationId
where fsi.OrganizationId={0}", orgId)).ToList();
        }
        public IEnumerable<FaultyStockInfo> GetAllFaultyMobilePartStockByParts(long warehouseId, long partsId, long orgId, long branchId, long modelId)
        {
            return _faultyStockInfoRepository.GetAll(info => info.SWarehouseId == warehouseId && info.PartsId == partsId && info.OrganizationId == orgId && info.BranchId == branchId && info.DescriptionId==modelId).ToList();
        }
        public FaultyStockInfo GetStockInfoOneById(long id, long orgId)
        {
            return _faultyStockInfoRepository.GetOneByOrg(s => s.FaultyStockInfoId == id && s.OrganizationId == orgId);
        }

        public FaultyStockInfo GetAllFaultyStockByStockIn(long modelId, long partsId, long orgId, long branchId)
        {
            return _faultyStockInfoRepository.GetOneByOrg(s => s.DescriptionId == modelId && s.PartsId == partsId && s.OrganizationId == orgId && s.BranchId == branchId);
        }
    }
}
