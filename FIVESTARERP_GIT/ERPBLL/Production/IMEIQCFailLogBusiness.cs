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
    public class IMEIQCFailLogBusiness : IIMEIQCFailLogBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly IMEIQCFailLogRepository _iMEIQCFailLogRepository;
        public IMEIQCFailLogBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._iMEIQCFailLogRepository = new IMEIQCFailLogRepository(this._productionDb);
        }

        public async Task<IEnumerable<IMEIQCFailLog>> GetAllRepairInByPackagingLineWithTime(long packagingId, DateTime time, long orgId)
        {
            return await _iMEIQCFailLogRepository.GetAllAsync(s => s.PackagingLineId == packagingId && DbFunctions.TruncateTime(s.EntryDate) == DbFunctions.TruncateTime(time) && s.OrganizationId == orgId);
        }
    }
}
