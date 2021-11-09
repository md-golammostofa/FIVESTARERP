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
    public class RepairOutBusiness : IRepairOutBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;

        //Repo
        private readonly RepairOutRepository _repairOutRepository;
        public RepairOutBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._repairOutRepository = new RepairOutRepository(this._productionDb);
        }

        public IEnumerable<RepairOut> GetAllRepairOutDataByAssemblyIdWithTimeWise(long assemlyId, DateTime time, long orgId)
        {
            return _repairOutRepository.GetAll(s => s.AssemblyLineId == assemlyId && DbFunctions.TruncateTime(s.EntryDate) == DbFunctions.TruncateTime(time) && s.OrganizationId == orgId);
        }
    }
}
