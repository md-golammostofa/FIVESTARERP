namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_add_tblSemiFinishGoodsWarehouseStockInfo_Detail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblSemiFinishGoodsWarehouseStockDetail",
                c => new
                    {
                        SemiFinishGoodsStockDetailId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(nullable: false),
                        DescriptionId = c.Long(nullable: false),
                        WarehouseId = c.Long(nullable: false),
                        ItemTypeId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        UnitId = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                        StockStatus = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        SemiFinishGoodsStockInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.SemiFinishGoodsStockDetailId);
            
            CreateTable(
                "dbo.tblSemiFinishGoodsWarehouseStockInfo",
                c => new
                    {
                        SemiFinishGoodsStockInfoId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(nullable: false),
                        DescriptionId = c.Long(nullable: false),
                        WarehouseId = c.Long(nullable: false),
                        ItemTypeId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        UnitId = c.Long(nullable: false),
                        StockInQty = c.Int(nullable: false),
                        StockOutQty = c.Int(nullable: false),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SemiFinishGoodsStockInfoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblSemiFinishGoodsWarehouseStockInfo");
            DropTable("dbo.tblSemiFinishGoodsWarehouseStockDetail");
        }
    }
}
