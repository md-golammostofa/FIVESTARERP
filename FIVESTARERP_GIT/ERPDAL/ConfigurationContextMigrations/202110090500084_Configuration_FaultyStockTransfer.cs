namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_FaultyStockTransfer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblFaultyStockTransferDetails",
                c => new
                    {
                        FSTDetailsId = c.Long(nullable: false, identity: true),
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
                        FSTInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.FSTDetailsId)
                .ForeignKey("dbo.tblFaultyStockTransferInfo", t => t.FSTInfoId, cascadeDelete: true)
                .Index(t => t.FSTInfoId);
            
            CreateTable(
                "dbo.tblFaultyStockTransferInfo",
                c => new
                    {
                        FSTInfoId = c.Long(nullable: false, identity: true),
                        TransferCode = c.String(),
                        StateStatus = c.String(),
                        BranchFrom = c.Long(nullable: false),
                        BranchTo = c.Long(nullable: false),
                        Remarks = c.String(),
                        BranchId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FSTInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblFaultyStockTransferDetails", "FSTInfoId", "dbo.tblFaultyStockTransferInfo");
            DropIndex("dbo.tblFaultyStockTransferDetails", new[] { "FSTInfoId" });
            DropTable("dbo.tblFaultyStockTransferInfo");
            DropTable("dbo.tblFaultyStockTransferDetails");
        }
    }
}
