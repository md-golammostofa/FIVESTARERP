namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_FaultyStockRepaired : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblFaultyStockRepairedDetails",
                c => new
                    {
                        FSRDetailsId = c.Long(nullable: false, identity: true),
                        ModelId = c.Long(nullable: false),
                        PartsId = c.Long(nullable: false),
                        TSId = c.Long(nullable: false),
                        RefCode = c.String(),
                        AssignQty = c.Int(nullable: false),
                        RepairedQty = c.Int(nullable: false),
                        RepairedDate = c.DateTime(),
                        ScrapedQty = c.Int(nullable: false),
                        ScrapedDate = c.DateTime(),
                        Remarks = c.String(),
                        BranchId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        FSRInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.FSRDetailsId)
                .ForeignKey("dbo.tblFaultyStockRepairedInfo", t => t.FSRInfoId, cascadeDelete: true)
                .Index(t => t.FSRInfoId);
            
            CreateTable(
                "dbo.tblFaultyStockRepairedInfo",
                c => new
                    {
                        FSRInfoId = c.Long(nullable: false, identity: true),
                        TSId = c.Long(nullable: false),
                        Code = c.String(),
                        StateStatus = c.String(),
                        AssignDate = c.DateTime(),
                        RepairedDate = c.DateTime(),
                        ReceiveDate = c.DateTime(),
                        ReceiveUserId = c.Long(),
                        Remarks = c.String(),
                        BranchId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FSRInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblFaultyStockRepairedDetails", "FSRInfoId", "dbo.tblFaultyStockRepairedInfo");
            DropIndex("dbo.tblFaultyStockRepairedDetails", new[] { "FSRInfoId" });
            DropTable("dbo.tblFaultyStockRepairedInfo");
            DropTable("dbo.tblFaultyStockRepairedDetails");
        }
    }
}
