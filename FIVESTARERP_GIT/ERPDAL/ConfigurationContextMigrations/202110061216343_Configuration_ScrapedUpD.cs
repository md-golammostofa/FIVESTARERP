namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_ScrapedUpD : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tblScrapStockDetail", "FaultyStockAssignTSId", c => c.Long());
            AlterColumn("dbo.tblScrapStockDetail", "ScrapStockInfoId", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblScrapStockDetail", "ScrapStockInfoId", c => c.Long(nullable: false));
            AlterColumn("dbo.tblScrapStockDetail", "FaultyStockAssignTSId", c => c.Long(nullable: false));
        }
    }
}
