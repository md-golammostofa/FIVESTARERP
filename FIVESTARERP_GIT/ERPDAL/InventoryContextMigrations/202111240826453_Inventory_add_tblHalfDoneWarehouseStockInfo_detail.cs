namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_add_tblHalfDoneWarehouseStockInfo_detail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblHalfDoneWarehouseStockDetail",
                c => new
                    {
                        HalfDoneStockDetailId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(),
                        AssemblyLineId = c.Long(),
                        QCId = c.Long(),
                        DescriptionId = c.Long(),
                        RepairLineId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        StockStatus = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        HalfDoneStockInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.HalfDoneStockDetailId);
            
            CreateTable(
                "dbo.tblHalfDoneWarehouseStockInfo",
                c => new
                    {
                        HalfDoneStockInfoId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(),
                        AssemblyLineId = c.Long(),
                        QCId = c.Long(),
                        DescriptionId = c.Long(),
                        RepairLineId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        StockInQty = c.Int(nullable: false),
                        StockOutQty = c.Int(nullable: false),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.HalfDoneStockInfoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblHalfDoneWarehouseStockInfo");
            DropTable("dbo.tblHalfDoneWarehouseStockDetail");
        }
    }
}
