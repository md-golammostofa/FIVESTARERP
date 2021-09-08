﻿using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
    public interface IFaultyStockInfoBusiness
    {
        IEnumerable<FaultyStockInfo> GetAllFaultyStockInfoByOrgId(long orgId, long branchId);
        FaultyStockInfo GetAllFaultyStockInfoByModelAndPartsIdAndCostPrice(long modelId, long partsId, double cprice, long orgId, long branchId);
        IEnumerable<FaultyStockInfoDTO> GetFaultyStockInfoByQuery(long? warehouseId, long? modelId, long? partsId, string lessOrEq, long orgId, long branchId);
        IEnumerable<FaultyStockInfoDTO> GetAllFaultyMobilePartsAndCode(long orgId);
        IEnumerable<FaultyStockInfo> GetAllFaultyMobilePartStockByParts(long warehouseId, long partsId, long orgId, long branchId,long modelId);
        FaultyStockInfo GetStockInfoOneById(long id, long orgId);
        FaultyStockInfo GetAllFaultyStockByStockIn(long modelId, long partsId, long orgId, long branchId);
    }
}
