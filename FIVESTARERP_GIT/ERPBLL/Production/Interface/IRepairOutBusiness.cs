using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IRepairOutBusiness
    {
        IEnumerable<RepairOut> GetAllRepairOutDataByAssemblyIdWithTimeWise(long assemlyId, DateTime time, long orgId);
    }
}
