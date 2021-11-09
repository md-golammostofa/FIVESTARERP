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
    public class QC3DetailBusiness : IQC3DetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly QC3DetailRepository _qC3DetailRepository;
        public QC3DetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._qC3DetailRepository = new QC3DetailRepository(this._productionDb);
        }
        public IEnumerable<QC3Detail> GetAllQC3ProblemByAssemblyId(long assemblyId, DateTime date, long orgId)
        {
            return _qC3DetailRepository.GetAll(s => s.AssemblyLineId == assemblyId && DbFunctions.TruncateTime(s.EntryDate) == DbFunctions.TruncateTime(date) && s.OrganizationId == orgId).ToList();
        }
    }
}
