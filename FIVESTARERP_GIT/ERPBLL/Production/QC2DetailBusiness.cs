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
    public class QC2DetailBusiness : IQC2DetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly QC2DetailRepository _qC2DetailRepository;
        public QC2DetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._qC2DetailRepository = new QC2DetailRepository(this._productionDb);
        }
        public IEnumerable<QC2Detail> GetAllQC2ProblemByAssemblyId(long assemblyId, DateTime date, long orgId)
        {
            return _qC2DetailRepository.GetAll(s => s.AssemblyLineId == assemblyId && DbFunctions.TruncateTime(s.EntryDate) == DbFunctions.TruncateTime(date) && s.OrganizationId == orgId).ToList();
        }
    }
}
