using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IMiniStockTransferToSemiFinishGoodsWarehouseBusiness
    {
        bool SaveMiniStockTransferToSemiFinishGoodsWarehouse(MiniStockTransferToSemiFinishGoodsWarehouseDTO dto, long userId, long orgId);
        IEnumerable<MiniStockTransferToSemiFinishGoodsWarehouseDTO> GetMiniStockTransferToSemiFinishGoodsWarehouseByQuery(long? floorId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId);
    }
}
