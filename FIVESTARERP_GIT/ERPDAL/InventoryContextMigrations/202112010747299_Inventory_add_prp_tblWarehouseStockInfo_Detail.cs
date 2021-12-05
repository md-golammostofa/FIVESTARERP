namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_add_prp_tblWarehouseStockInfo_Detail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblWarehouseStockDetails", "GoodStockQty", c => c.Int(nullable: false));
            AddColumn("dbo.tblWarehouseStockDetails", "ManMadeFaultyQty", c => c.Int(nullable: false));
            AddColumn("dbo.tblWarehouseStockDetails", "ChinaFaultyQty", c => c.Int(nullable: false));
            AddColumn("dbo.tblWarehouseStockInfo", "ManMadeFaultyStockInQty", c => c.Int());
            AddColumn("dbo.tblWarehouseStockInfo", "ManMadeFaultyStockOutQty", c => c.Int());
            AddColumn("dbo.tblWarehouseStockInfo", "ChinaMadeFaultyStockInQty", c => c.Int());
            AddColumn("dbo.tblWarehouseStockInfo", "ChinaMadeFaultyStockOutQty", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblWarehouseStockInfo", "ChinaMadeFaultyStockOutQty");
            DropColumn("dbo.tblWarehouseStockInfo", "ChinaMadeFaultyStockInQty");
            DropColumn("dbo.tblWarehouseStockInfo", "ManMadeFaultyStockOutQty");
            DropColumn("dbo.tblWarehouseStockInfo", "ManMadeFaultyStockInQty");
            DropColumn("dbo.tblWarehouseStockDetails", "ChinaFaultyQty");
            DropColumn("dbo.tblWarehouseStockDetails", "ManMadeFaultyQty");
            DropColumn("dbo.tblWarehouseStockDetails", "GoodStockQty");
        }
    }
}
