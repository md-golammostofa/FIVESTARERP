namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_DustStock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblDustStockDetails",
                c => new
                    {
                        DetailsId = c.Long(nullable: false, identity: true),
                        ModelId = c.Long(nullable: false),
                        PartsId = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                        StateStatus = c.String(),
                        Remarks = c.String(),
                        BranchId = c.Long(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        DustStockInfo_InfoId = c.Long(),
                    })
                .PrimaryKey(t => t.DetailsId)
                .ForeignKey("dbo.tblDustStockInfo", t => t.DustStockInfo_InfoId)
                .Index(t => t.DustStockInfo_InfoId);
            
            CreateTable(
                "dbo.tblDustStockInfo",
                c => new
                    {
                        InfoId = c.Long(nullable: false, identity: true),
                        ModelId = c.Long(nullable: false),
                        PartsId = c.Long(nullable: false),
                        StockInQty = c.Int(nullable: false),
                        StockOutQty = c.Int(nullable: false),
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
            DropForeignKey("dbo.tblDustStockDetails", "DustStockInfo_InfoId", "dbo.tblDustStockInfo");
            DropIndex("dbo.tblDustStockDetails", new[] { "DustStockInfo_InfoId" });
            DropTable("dbo.tblDustStockInfo");
            DropTable("dbo.tblDustStockDetails");
        }
    }
}
