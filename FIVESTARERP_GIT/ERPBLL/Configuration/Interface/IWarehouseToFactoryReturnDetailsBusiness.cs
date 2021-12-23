using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
    public interface IWarehouseToFactoryReturnDetailsBusiness
    {
        IEnumerable<WarehouseToFactoryReturnDetailsDTO> GetDetailsDataByInfoId(long infoId, long orgId, long branchId);
        IEnumerable<WarehouseToFactoryReturnDetailsDTO> GetDetailsAllData(long orgId, long branchId);
    }
}
