namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_add_tblBatteryWriteByIMEILog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblBatteryWriteByIMEILog",
                c => new
                    {
                        BatteryWriteLogId = c.Long(nullable: false, identity: true),
                        IMEI = c.String(),
                        BatteryCode = c.String(),
                        CodeId = c.Long(),
                        CodeNo = c.String(),
                        ProductionFloorId = c.Long(),
                        ProductionFloorName = c.String(),
                        AssemblyId = c.Long(),
                        AssemblyLineName = c.String(),
                        PackagingLineId = c.Long(),
                        PackagingLineName = c.String(),
                        QCLineId = c.Long(),
                        QCLineName = c.String(),
                        DescriptionId = c.Long(),
                        ModelName = c.String(),
                        ColorId = c.Long(),
                        ColorName = c.String(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        ItemName = c.String(),
                        StateStatus = c.String(),
                        ReferenceNumber = c.String(),
                        ReferenceId = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BatteryWriteLogId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblBatteryWriteByIMEILog");
        }
    }
}
