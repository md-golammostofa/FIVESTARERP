namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_add_prp_StockInfo_in_tblWarehouseStockDetail : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tblWarehouseStockDetails", "WarehouseStockInfo_StockInfoId", "dbo.tblWarehouseStockInfo");
            DropIndex("dbo.tblWarehouseStockDetails", new[] { "WarehouseStockInfo_StockInfoId" });
            AddColumn("dbo.tblWarehouseStockDetails", "StockInfoId", c => c.Long(nullable: false));
            DropColumn("dbo.tblWarehouseStockDetails", "WarehouseStockInfo_StockInfoId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblWarehouseStockDetails", "WarehouseStockInfo_StockInfoId", c => c.Long());
            DropColumn("dbo.tblWarehouseStockDetails", "StockInfoId");
            CreateIndex("dbo.tblWarehouseStockDetails", "WarehouseStockInfo_StockInfoId");
            AddForeignKey("dbo.tblWarehouseStockDetails", "WarehouseStockInfo_StockInfoId", "dbo.tblWarehouseStockInfo", "StockInfoId");
        }
    }
}
