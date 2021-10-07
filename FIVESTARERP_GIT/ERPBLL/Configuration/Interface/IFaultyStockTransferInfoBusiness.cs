using ERPBO.Configuration.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
    public interface IFaultyStockTransferInfoBusiness
    {
        IEnumerable<FaultyStockTransferInfo> GetAllStockInfo(long orgId, long branchId);
        FaultyStockTransferInfo GetOneByOneInfo(long infoId, long orgId, long branchId);
    }
}
