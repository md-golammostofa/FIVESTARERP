using ERPBLL.Production.Interface;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class QC3InfoBusiness : IQC3InfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        public QC3InfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
        }
    }
}
