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
    public class QC1DetailBusiness : IQC1DetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly QC1DetailRepository _qC1DetailRepository;
        public QC1DetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._qC1DetailRepository = new QC1DetailRepository(this._productionDb);
        }
        public IEnumerable<QC1Detail> GetAllQC1ProblemByAssemblyId(long assemblyId, DateTime date, long orgId)
        {
            return _qC1DetailRepository.GetAll(s => s.AssemblyLineId == assemblyId && DbFunctions.TruncateTime(s.EntryDate) == DbFunctions.TruncateTime(date) && s.OrganizationId == orgId).ToList();
        }
    }
}
