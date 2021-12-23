using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
    public interface IWarehouseToFactoryReturnInfoBusiness
    {
        bool SaveStockReturnWarehouseToFactory(List<WarehouseToFactoryReturnDetailsDTO> dto, long userId, long orgId, long branchId);
        bool SaveStockReturnWarehouseToFactoryAndStockOut(List<MobilePartStockDetailDTO> mobilePartStockDetailDTO, long orgId, long branchId, long userId);
        IEnumerable<WarehouseToFactoryReturnInfoDTO> GetAllReturnList(long orgId, long branchId);
    }
}
