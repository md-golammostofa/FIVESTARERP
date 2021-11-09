namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_add_tblRepairIn_tblRepairOut : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblRepairIn",
                c => new
                    {
                        RepairInId = c.Long(nullable: false, identity: true),
                        QRTRInfoId = c.Long(nullable: false),
                        TransferId = c.Long(),
                        TransferCode = c.String(),
                        FloorId = c.Long(nullable: false),
                        AssemblyLineId = c.Long(nullable: false),
                        QCLineId = c.Long(nullable: false),
                        SubQCId = c.Long(),
                        RepairLineId = c.Long(nullable: false),
                        QRCode = c.String(),
                        DescriptionId = c.Long(nullable: false),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        StateStatus = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RepairInId);
            
            CreateTable(
                "dbo.tblRepairOut",
                c => new
                    {
                        RepairOutId = c.Long(nullable: false, identity: true),
                        QRTRInfoId = c.Long(nullable: false),
                        TransferId = c.Long(),
                        TransferCode = c.String(),
                        FloorId = c.Long(nullable: false),
                        AssemblyLineId = c.Long(nullable: false),
                        QCLineId = c.Long(nullable: false),
                        SubQCId = c.Long(),
                        RepairLineId = c.Long(nullable: false),
                        QRCode = c.String(),
                        DescriptionId = c.Long(nullable: false),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        StateStatus = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RepairOutId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblRepairOut");
            DropTable("dbo.tblRepairIn");
        }
    }
}
