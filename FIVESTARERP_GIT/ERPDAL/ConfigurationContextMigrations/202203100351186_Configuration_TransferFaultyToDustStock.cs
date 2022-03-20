namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_TransferFaultyToDustStock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblTransferFaultyToDustDetails",
                c => new
                    {
                        DetailsId = c.Long(nullable: false, identity: true),
                        ModelId = c.Long(nullable: false),
                        PartsId = c.Long(nullable: false),
                        CostPrice = c.Double(nullable: false),
                        SellPrice = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                        RefCode = c.String(),
                        Remarks = c.String(),
                        BranchId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        InfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.DetailsId)
                .ForeignKey("dbo.tblTransferFaultyToDustInfo", t => t.InfoId, cascadeDelete: true)
                .Index(t => t.InfoId);
            
            CreateTable(
                "dbo.tblTransferFaultyToDustInfo",
                c => new
                    {
                        InfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(),
                        StateStatus = c.String(),
                        Remarks = c.String(),
                        BranchId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.InfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblTransferFaultyToDustDetails", "InfoId", "dbo.tblTransferFaultyToDustInfo");
            DropIndex("dbo.tblTransferFaultyToDustDetails", new[] { "InfoId" });
            DropTable("dbo.tblTransferFaultyToDustInfo");
            DropTable("dbo.tblTransferFaultyToDustDetails");
        }
    }
}
