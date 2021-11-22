namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_add_tblMiniStockRequisitionToWarehouse : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblMiniStockRequisitionToSemiFinishGoodsWarehouseDetail",
                c => new
                    {
                        RequisitionDetailId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(nullable: false),
                        DescriptionId = c.Long(nullable: false),
                        WarehouseId = c.Long(nullable: false),
                        ItemTypeId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        UnitId = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        RequisitionInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.RequisitionDetailId);
            
            CreateTable(
                "dbo.tblMiniStockRequisitionToSemiFinishGoodsWarehouseInfo",
                c => new
                    {
                        RequisitionInfoId = c.Long(nullable: false, identity: true),
                        RequisitionCode = c.String(),
                        TotalQuantity = c.Int(nullable: false),
                        StateStatus = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RequisitionInfoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblMiniStockRequisitionToSemiFinishGoodsWarehouseInfo");
            DropTable("dbo.tblMiniStockRequisitionToSemiFinishGoodsWarehouseDetail");
        }
    }
}
