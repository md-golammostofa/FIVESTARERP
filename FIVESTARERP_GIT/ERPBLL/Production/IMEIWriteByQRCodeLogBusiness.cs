using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class IMEIWriteByQRCodeLogBusiness : IIMEIWriteByQRCodeLogBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly IMEIWriteByQRCodeLogRepository _iMEIWriteByQRCodeLogRepository;
        public IMEIWriteByQRCodeLogBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._iMEIWriteByQRCodeLogRepository = new IMEIWriteByQRCodeLogRepository(this._productionDb);
        }
        public async Task<IMEIWriteByQRCodeLog>  GetLogInfoByIMEI(string imei, long orgId)
        {
            return await _iMEIWriteByQRCodeLogRepository.GetOneByOrgAsync(s => s.IMEI.Contains(imei) && s.OrganizationId == orgId);
        }
        public async Task<IEnumerable<IMEIWriteByQRCodeLog>> GetAllIMEIWriteByPackagingLineWithTime(long packagingId, DateTime time, long orgId)
        {
            return await _iMEIWriteByQRCodeLogRepository.GetAllAsync(s => s.PackagingLineId == packagingId && DbFunctions.TruncateTime(s.EntryDate) == DbFunctions.TruncateTime(time) && s.OrganizationId == orgId);
        }
    }
}
