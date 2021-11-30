using ERPBLL.Common;
using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class WeightCheckedIMEILogBusiness : IWeightCheckedIMEILogBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly WeightCheckedIMEILogRepository _weightCheckedIMEILogRepository;
        public WeightCheckedIMEILogBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            _weightCheckedIMEILogRepository = new WeightCheckedIMEILogRepository(this._productionDb);
        }
        public IEnumerable<WeightCheckedIMEILog> GetAllWeightCheckedInfoByUserId(long userId, long orgId, DateTime date)
        {
            return _weightCheckedIMEILogRepository.GetAll(s => s.EUserId == userId && s.OrganizationId == orgId && DbFunctions.TruncateTime(s.EntryDate) == DbFunctions.TruncateTime(date)).ToList();
        }
        public IEnumerable<WeightCheckedIMEILogDTO> GetAllDataByDateWise(string fromDate, string toDate, string imei, long userId)
        {
            IEnumerable<WeightCheckedIMEILogDTO> iMEIDTOs = new List<WeightCheckedIMEILogDTO>();
            iMEIDTOs = this._productionDb.Db.Database.SqlQuery<WeightCheckedIMEILogDTO>(QueryForWeightCheckedDateWiseData(fromDate, toDate, imei, userId)).ToList();
            return iMEIDTOs;
        }

        private string QueryForWeightCheckedDateWiseData(string fromDate, string toDate, string imei, long userId)
        {
            string query = string.Empty;
            string param = string.Empty;
            string i = imei.Trim();
            if (!string.IsNullOrEmpty(i) && i != "")
            {
                param += string.Format(@" and bs.IMEI Like'%{0}%'", i);
            }

            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(bs.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(bs.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(bs.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@" Select bs.EntryDate, au.UserName 'EUserName', bs.StateStatus, bs.IMEI, bs.WeightCheckedIMEILogId, bs.Remarks, bs.CodeNo, bs.ReferenceNumber
From tblWeightCheckedIMEILog bs
Left Join [ControlPanel-MC].dbo.tblApplicationUsers au on bs.EUserId = au.UserId
Where bs.EUserId= {0}{1}", userId, Utility.ParamChecker(param));
            return query;
        }
    }
}
