using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IHalfDoneStockTransferToWarehouseInfoBusiness
    {
        IEnumerable<HalfDoneStockTransferToWarehouseInfo> GetAllTransferList(long orgId);
        HalfDoneStockTransferToWarehouseInfo GetTransferInfoById(long id, long orgId);
        bool SaveHalfDoneStockTransferToWarehouse(List<HalfDoneStockTransferToWarehouseDetailDTO> dTOs, int totalQty, long userId, long orgId);
        IEnumerable<HalfDoneStockTransferToWarehouseInfoDTO> GetHalfDoneStockTransferInfoByQuery(string reqNo, string status, string lessOrEq, string fromDate, string toDate, long orgId);
        bool UpdateHalfDoneTransferStatusForHalfDoneStockIn(long transferInfoId, long userId, long orgId);
        bool UpdateHalfDoneTransferStatusForHalfDoneStockOut(long transferInfoId, long userId, long orgId);
        bool UpdateHalfDoneTransferStatusForMiniStock(long transferInfoId, long userId, long orgId);
    }
}
