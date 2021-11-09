using ERPBLL.Production.Interface;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class QC1InfoBusiness : IQC1InfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        public QC1InfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
        }
    }
}
