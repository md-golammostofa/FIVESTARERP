namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_PartsTransferHTCInfoDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblPartsTransferHToCInfo",
                c => new
                    {
                        InfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(),
                        StateStatus = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        BranchId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.InfoId);
            
            CreateTable(
                "dbo.tblPartsTransferHToCDetails",
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
                        OrganizationId = c.Long(nullable: false),
                        BranchId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        InfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.DetailsId)
                .ForeignKey("dbo.tblPartsTransferHToCInfo", t => t.InfoId, cascadeDelete: true)
                .Index(t => t.InfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblPartsTransferHToCDetails", "InfoId", "dbo.tblPartsTransferHToCInfo");
            DropIndex("dbo.tblPartsTransferHToCDetails", new[] { "InfoId" });
            DropTable("dbo.tblPartsTransferHToCDetails");
            DropTable("dbo.tblPartsTransferHToCInfo");
        }
    }
}
