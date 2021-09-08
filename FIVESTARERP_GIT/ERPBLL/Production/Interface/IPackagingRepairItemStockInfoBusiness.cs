﻿using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IPackagingRepairItemStockInfoBusiness
    {
        PackagingRepairItemStockInfo GetPackagingRepairItemStockInfoByPackagingLineAndModelAndItem(long floorId, long packagingLine, long itemId, long modelId, long orgId);
        Task<PackagingRepairItemStockInfo> GetPackagingRepairItemStockInfoByPackagingLineAndModelAndItemAsync(long floorId, long packagingLine, long itemId, long modelId, long orgId);
        IEnumerable<PackagingRepairItemStockInfoDTO> GetPackagingRepairItemStockInfosByQuery(long? floorId, long? packagingLine, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId);
    }
}
