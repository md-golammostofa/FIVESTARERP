using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class BatteryWriteByIMEILogBusiness : IBatteryWriteByIMEILogBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly BatteryWriteByIMEILogRepository _batteryWriteByIMEILogRepository;
        public BatteryWriteByIMEILogBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._batteryWriteByIMEILogRepository = new BatteryWriteByIMEILogRepository(this._productionDb);
        }
        public async Task<BatteryWriteByIMEILog> GetLogInfoByIMEI(string imei, long orgId)
        {
            return await _batteryWriteByIMEILogRepository.GetOneByOrgAsync(s => s.IMEI.Contains(imei) && s.OrganizationId == orgId);
        }
    }
}
