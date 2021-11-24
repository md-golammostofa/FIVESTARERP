using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IHalfDoneStockTransferToWarehouseDetailBusiness
    {
        IEnumerable<HalfDoneStockTransferToWarehouseDetail> GetAllTransferDetails(long orgId);
        IEnumerable<HalfDoneStockTransferToWarehouseDetailDTO> GetHalfDoneTransferDetailByQuery(long? transferInfoId, long? floorId, long? assemblyId, long? qcId, long? repairId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, long orgId);
    }
}
