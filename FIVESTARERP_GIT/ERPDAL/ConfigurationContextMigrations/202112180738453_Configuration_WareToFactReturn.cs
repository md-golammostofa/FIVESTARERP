namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_WareToFactReturn : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblWarehouseToFactoryReturnDetails",
                c => new
                    {
                        WTFDetailsId = c.Long(nullable: false, identity: true),
                        ReferenceCode = c.String(),
                        ModelId = c.Long(nullable: false),
                        PartsId = c.Long(nullable: false),
                        CostPrice = c.Double(nullable: false),
                        SellPrice = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        BranchId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        WTFInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.WTFDetailsId)
                .ForeignKey("dbo.tblWarehouseToFactoryReturnInfo", t => t.WTFInfoId, cascadeDelete: true)
                .Index(t => t.WTFInfoId);
            
            CreateTable(
                "dbo.tblWarehouseToFactoryReturnInfo",
                c => new
                    {
                        WTFInfoId = c.Long(nullable: false, identity: true),
                        ReturnCode = c.String(),
                        StateStatus = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        BranchId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.WTFInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblWarehouseToFactoryReturnDetails", "WTFInfoId", "dbo.tblWarehouseToFactoryReturnInfo");
            DropIndex("dbo.tblWarehouseToFactoryReturnDetails", new[] { "WTFInfoId" });
            DropTable("dbo.tblWarehouseToFactoryReturnInfo");
            DropTable("dbo.tblWarehouseToFactoryReturnDetails");
        }
    }
}
