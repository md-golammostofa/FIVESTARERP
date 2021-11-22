using ERPBLL.Inventory.Interface;
using ERPDAL.InventoryDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
    public class SemiFinishGoodsWarehouseStockDetailBusiness : ISemiFinishGoodsWarehouseStockDetailBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryDB;
        public SemiFinishGoodsWarehouseStockDetailBusiness(IInventoryUnitOfWork inventoryDB)
        {
            this._inventoryDB = inventoryDB;
        }
    }
}
