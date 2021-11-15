using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IMiniStockTransferToWarehouseInfoBusiness
    {
        IEnumerable<MiniStockTransferToWarehouseInfo> GetAllTransferList(long orgId);
        bool SaveMiniStockTransferToWarehouse(MiniStockTransferToWarehouseInfoDTO dto, long orgId, long userId);
    }
}
