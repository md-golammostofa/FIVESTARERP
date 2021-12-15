using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IWeightCheckedIMEILogBusiness
    {
        IEnumerable<WeightCheckedIMEILog> GetAllWeightCheckedInfoByUserId(long userId, long orgId, DateTime date);
        IEnumerable<WeightCheckedIMEILogDTO> GetAllDataByDateWise(string fromDate, string toDate, string imei, long userId);
        Task<bool> SaveIMEIStatusForWeightCheck(string imei, long orgId, long userId);
    }
}
