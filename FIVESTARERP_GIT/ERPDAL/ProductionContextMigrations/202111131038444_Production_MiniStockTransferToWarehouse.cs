namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_MiniStockTransferToWarehouse : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblMiniStockTransferToWarehouseDetails",
                c => new
                    {
                        MSTWDetailsId = c.Long(nullable: false, identity: true),
                        FloorId = c.Long(),
                        AssemblyLineId = c.Long(),
                        DescriptionId = c.Long(),
                        RepairLineId = c.Long(),
                        WarehouseId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        MSTWInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.MSTWDetailsId)
                .ForeignKey("dbo.tblMiniStockTransferToWarehouseInfo", t => t.MSTWInfoId, cascadeDelete: true)
                .Index(t => t.MSTWInfoId);
            
            CreateTable(
                "dbo.tblMiniStockTransferToWarehouseInfo",
                c => new
                    {
                        MSTWInfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(),
                        FloorId = c.Long(),
                        AssemblyLineId = c.Long(),
                        DescriptionId = c.Long(),
                        RepairLineId = c.Long(),
                        WarehouseId = c.Long(),
                        TransferStatus = c.String(),
                        ReturnStatus = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.MSTWInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblMiniStockTransferToWarehouseDetails", "MSTWInfoId", "dbo.tblMiniStockTransferToWarehouseInfo");
            DropIndex("dbo.tblMiniStockTransferToWarehouseDetails", new[] { "MSTWInfoId" });
            DropTable("dbo.tblMiniStockTransferToWarehouseInfo");
            DropTable("dbo.tblMiniStockTransferToWarehouseDetails");
        }
    }
}
