using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IMiniStockRequisitionToSemiFinishGoodsWarehouseInfoBusiness
    {
        IEnumerable<MiniStockRequisitionToSemiFinishGoodsWarehouseInfo> GetAllRequisitionInfoByOrgId(long orgId);
        bool SaveMiniStockRequisitionToWarehouse(List<MiniStockRequisitionToSemiFinishGoodsWarehouseDetailDTO> dTOs, int totalQty, long userId, long orgId);
        IEnumerable<MiniStockRequisitionToSemiFinishGoodsWarehouseInfoDTO> GetMiniStockRequisitionInfoByQuery(string reqNo, string status, string lessOrEq, string fromDate, string toDate, long orgId);
        MiniStockRequisitionToSemiFinishGoodsWarehouseInfo GetRequisitionInfoById(long id, long orgId);
        bool UpdateMiniWarehouseRequisitionStatusForSemiFinishGoodsStock(long reqInfoId, long userId, long orgId);
        bool UpdateMiniWarehouseRequisitionStatusForMiniWarehouseStock(long reqInfoId, long userId, long orgId);
    }
}
