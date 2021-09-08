﻿using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
    public interface IWarehouseFaultyStockDetailBusiness
    {
        IEnumerable<WarehouseFaultyStockDetail> GetWarehouseFaultyStockDetails(long orgId);
        WarehouseFaultyStockDetail GetWarehouseFaultyStockDetailById(long orgId,long stockDetailId);
        bool SaveWarehouseFaultyStockIn(List<WarehouseFaultyStockDetailDTO> repairStockDetailDTOs,long orgId, long userId);
        IEnumerable<WarehouseFaultyStockDetailListDTO> GetWarehouseFaultyStockDetailList(long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate,string refNum,long orgId, string returnType, string faultyCase);
    }
}
