using ERPBO.Inventory.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
    public interface IWarehouseFaultyStockInfoBusiness
    {
        IEnumerable<WarehouseFaultyStockInfo> GetWarehouseFaultyStockInfos(long orgId);
        WarehouseFaultyStockInfo GetWarehouseFaultyStockInfoById(long orgId, long stockInfoId);
    }
}
