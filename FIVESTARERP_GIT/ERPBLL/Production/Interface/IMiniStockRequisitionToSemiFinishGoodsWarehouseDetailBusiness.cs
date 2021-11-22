using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IMiniStockRequisitionToSemiFinishGoodsWarehouseDetailBusiness
    {
        IEnumerable<MiniStockRequisitionToSemiFinishGoodsWarehouseDetail> GetAllRequisitionDetails(long orgId);
        IEnumerable<MiniStockRequisitionToSemiFinishGoodsWarehouseDetailDTO> GetMiniStockRequisitionDetailByQuery(long? reqInfoId, long? floorId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, long orgId);
    }
}
