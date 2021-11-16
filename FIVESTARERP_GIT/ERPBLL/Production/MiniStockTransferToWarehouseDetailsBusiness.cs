using ERPBLL.Production.Interface;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class MiniStockTransferToWarehouseDetailsBusiness: IMiniStockTransferToWarehouseDetailsBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly MiniStockTransferToWarehouseDetailsRepository _transferToWarehouseDetailsRepository;
        public MiniStockTransferToWarehouseDetailsBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._transferToWarehouseDetailsRepository = new MiniStockTransferToWarehouseDetailsRepository(this._productionDb);
        }
    }
}
