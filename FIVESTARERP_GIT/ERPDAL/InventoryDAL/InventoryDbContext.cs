﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPBO.Inventory.DomainModels;

namespace ERPDAL.InventoryDAL
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext():base("Inventory")
        {

        }
        public DbSet<Description> tblDescriptions { get; set; }
        public DbSet<Warehouse> tblWarehouses { get; set; }
        public DbSet<ItemType> tblItemTypes { get; set; }
        public DbSet<Unit> tblUnits { get; set; }
        public DbSet<Item> tblItems { get; set; }
        public DbSet<WarehouseStockInfo> tblWarehouseStockInfo { get; set; }
        public DbSet<WarehouseStockDetail> tblWarehouseStockDetails { get; set; }
        public DbSet<WarehouseFaultyStockInfo> tblRepairStockInfo { get; set; }
        public DbSet<WarehouseFaultyStockDetail> tblRepairStockDetails { get; set; }
        public DbSet<ItemPreparationInfo> tblItemPreparationInfo { get; set; }
        public DbSet<ItemPreparationDetail> tblItemPreparationDetail { get; set; }
        public DbSet<Supplier> tblSupplier { get; set; }
        public DbSet<IQC> tblIQCList { get; set; }
        public DbSet<IQCItemReqInfoList> tblIQCItemReqInfoList { get; set; }
        public DbSet<IQCItemReqDetailList> tblIQCItemReqDetailList { get; set; }
        public DbSet <IQCStockDetail> tblIQCStockDetails { get; set; }
        public DbSet<IQCStockInfo> tblIQCStockInfo { get; set; }
        public DbSet<Category> tblCategory { get; set; }
        public DbSet<Brand> tblBrand { get; set; }
        public DbSet<BrandCategories> tblBrandCategories { get; set; }
        public DbSet<Color> tblColors { get; set; }
        public DbSet<ModelColors> tblModelColors { get; set; }
        public DbSet<HandSetStock> tblHandSetStock { get; set; }
        public DbSet<StockTransferInfoMToM> tblSTransferInfoMToM { get; set; }
        public DbSet<StockTransferDetailsMToM> tblSTransferDetailsMToM { get; set; }
    }
}
