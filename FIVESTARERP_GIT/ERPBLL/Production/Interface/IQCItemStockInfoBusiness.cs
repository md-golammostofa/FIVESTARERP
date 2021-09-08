﻿using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IQCItemStockInfoBusiness
    {
        IEnumerable<QCItemStockInfo> GetQCItemStocks(long orgId);
        QCItemStockInfo GetQCItemStockInfById(long qcId, long modelId, long itemId, long orgId);
        QCItemStockInfo GetQCItemStockInfoByFloorAndQcAndModelAndItem(long floorId, long assemblyId,long qcId, long modelId, long itemId, long orgId);
        Task<QCItemStockInfo> GetQCItemStockInfoByFloorAndQcAndModelAndItemAsync(long floorId, long assemblyId,long qcId, long modelId, long itemId, long orgId);

        IEnumerable<QCItemStockInfoDTO> GetQCItemStockInfosByQuery(long? floorId, long? assemblyId, long? qCId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId,long orgId);
    }
}
