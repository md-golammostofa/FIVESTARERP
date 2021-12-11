using ERPBLL.Production.Interface;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class IMEIQCPassLogBusiness : IIMEIQCPassLogBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        public IMEIQCPassLogBusiness(IProductionUnitOfWork productionDb)
        {

        }
    }
}
