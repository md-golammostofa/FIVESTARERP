using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IRepairInBusiness
    {
        IEnumerable<RepairIn> GetAllRepairInDataByAssemblyIdWithTimeWise(long assemlyId, DateTime time, long orgId);
    }
}
