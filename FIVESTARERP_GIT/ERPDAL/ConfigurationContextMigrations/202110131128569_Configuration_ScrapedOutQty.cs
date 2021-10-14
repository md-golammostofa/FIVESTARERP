namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_ScrapedOutQty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblScrapStockInfo", "ScrapOutQty", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblScrapStockInfo", "ScrapOutQty");
        }
    }
}
