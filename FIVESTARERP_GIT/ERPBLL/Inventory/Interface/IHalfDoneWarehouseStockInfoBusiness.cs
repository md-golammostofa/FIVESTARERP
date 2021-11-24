using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
    public interface IHalfDoneWarehouseStockInfoBusiness
    {
        IEnumerable<HalfDoneWarehouseStockInfo> GetAllHalfDoneStockInfoByOrgId(long orgId);
        bool SaveHalfDoneWarehouseStockIn(List<HalfDoneWarehouseStockDetailDTO> dTOs, long userId, long orgId);
        bool SaveHalfDoneWarehouseStockOut(List<HalfDoneWarehouseStockDetailDTO> dTOs, long userId, long orgId);
        IEnumerable<HalfDoneWarehouseStockInfoDTO> GetHalfDoneStockByQuery(long? floorId, long? assemblyId, long? qcId, long? repairId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId);
    }
}
