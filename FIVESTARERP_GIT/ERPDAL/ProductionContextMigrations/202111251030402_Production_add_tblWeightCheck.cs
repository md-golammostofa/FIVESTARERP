namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_add_tblWeightCheck : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblWeightCheckedIMEILog",
                c => new
                    {
                        WeightCheckedIMEILogId = c.Long(nullable: false, identity: true),
                        CodeId = c.Long(),
                        CodeNo = c.String(maxLength: 200),
                        IMEI = c.String(maxLength: 300),
                        ProductionFloorId = c.Long(),
                        AssemblyId = c.Long(),
                        DescriptionId = c.Long(),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        StateStatus = c.String(maxLength: 100),
                        ReferenceNumber = c.String(maxLength: 200),
                        ReferenceId = c.String(),
                        Remarks = c.String(maxLength: 200),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.WeightCheckedIMEILogId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblWeightCheckedIMEILog");
        }
    }
}
