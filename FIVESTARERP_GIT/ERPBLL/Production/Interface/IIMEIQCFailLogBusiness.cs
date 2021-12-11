using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IIMEIQCFailLogBusiness
    {
        Task<IEnumerable<IMEIQCFailLog>> GetAllRepairInByPackagingLineWithTime(long packagingId, DateTime time, long orgId);
    }
}
