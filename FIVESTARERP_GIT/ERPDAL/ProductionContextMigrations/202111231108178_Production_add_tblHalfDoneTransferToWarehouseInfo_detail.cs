namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_add_tblHalfDoneTransferToWarehouseInfo_detail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblHalfDoneStockTransferToWarehouseDetail",
                c => new
                    {
                        HalfDoneTransferDetailId = c.Long(nullable: false, identity: true),
                        ProductionFloorId = c.Long(),
                        AssemblyLineId = c.Long(),
                        QCId = c.Long(),
                        DescriptionId = c.Long(),
                        RepairLineId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        HalfDoneTransferInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.HalfDoneTransferDetailId);
            
            CreateTable(
                "dbo.tblHalfDoneStockTransferToWarehouseInfo",
                c => new
                    {
                        HalfDoneTransferInfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(),
                        StateStatus = c.String(),
                        TotalQuantity = c.Int(nullable: false),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.HalfDoneTransferInfoId);
        }
        
        public override void Down()
        {
            DropTable("dbo.tblHalfDoneStockTransferToWarehouseInfo");
            DropTable("dbo.tblHalfDoneStockTransferToWarehouseDetail");
        }
    }
}
