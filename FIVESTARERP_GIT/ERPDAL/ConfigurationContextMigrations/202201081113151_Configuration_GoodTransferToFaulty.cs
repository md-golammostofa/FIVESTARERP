namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_GoodTransferToFaulty : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblGoodToFaultyTransferDetails",
                c => new
                    {
                        GTFTDetailsId = c.Long(nullable: false, identity: true),
                        ModelId = c.Long(nullable: false),
                        PartsId = c.Long(nullable: false),
                        CostPrice = c.Double(nullable: false),
                        SellPrice = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Remarks = c.String(),
                        BranchId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        GTFTInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.GTFTDetailsId)
                .ForeignKey("dbo.tblGoodToFaultyTransferInfo", t => t.GTFTInfoId, cascadeDelete: true)
                .Index(t => t.GTFTInfoId);
            
            CreateTable(
                "dbo.tblGoodToFaultyTransferInfo",
                c => new
                    {
                        GTFTInfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.Long(nullable: false),
                        TransferStatus = c.Long(nullable: false),
                        Remarks = c.String(),
                        BranchId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.GTFTInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblGoodToFaultyTransferDetails", "GTFTInfoId", "dbo.tblGoodToFaultyTransferInfo");
            DropIndex("dbo.tblGoodToFaultyTransferDetails", new[] { "GTFTInfoId" });
            DropTable("dbo.tblGoodToFaultyTransferInfo");
            DropTable("dbo.tblGoodToFaultyTransferDetails");
        }
    }
}
