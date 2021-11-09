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
    public class RepairInBusiness : IRepairInBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;

        //Repo
        private readonly RepairInRepository _repairInRepository;
        public RepairInBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            _repairInRepository = new RepairInRepository(this._productionDb);
        }

        public IEnumerable<RepairIn> GetAllRepairInDataByAssemblyIdWithTimeWise(long assemlyId, DateTime time, long orgId)
        {
            return _repairInRepository.GetAll(s => s.AssemblyLineId == assemlyId && DbFunctions.TruncateTime(s.EntryDate) == DbFunctions.TruncateTime(time) && s.OrganizationId == orgId);
        }
    }
}
