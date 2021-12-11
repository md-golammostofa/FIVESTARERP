using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IIMEIWriteByQRCodeLogBusiness
    {
         Task<IMEIWriteByQRCodeLog> GetLogInfoByIMEI(string imei, long orgId);
        Task<IEnumerable<IMEIWriteByQRCodeLog>> GetAllIMEIWriteByPackagingLineWithTime(long packagingId, DateTime time, long orgId);
    }
}
