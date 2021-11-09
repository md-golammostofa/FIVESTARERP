using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface IDustStockInfoBusiness
    {
        DustStockInfo GetPartsByModel(long modelId, long partsId, long orgId, long branchId);
        IEnumerable<DustStockInfo> GetAllStockForList(long orgId, long branchId);
    }
}
