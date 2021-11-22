using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
    public interface ISemiFinishGoodsWarehouseStockInfoBusiness
    {
        IEnumerable<SemiFinishGoodsWarehouseStockInfo> GetAllSemiFinishGoodStockByOrgId(long orgId);
        bool SaveSemiFinishGoodStockIn(List<SemiFinishGoodsWarehouseStockDetailDTO> dTOs, long userId, long orgId);
        bool SaveSemiFinishGoodStockOut(List<SemiFinishGoodsWarehouseStockDetailDTO> dTOs, long userId, long orgId);
        IEnumerable<SemiFinishGoodsWarehouseStockInfoDTO> GetSemiFinishGoodsStockByQuery(long? floorId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, string lessOrEq, long orgId);
    }
}
