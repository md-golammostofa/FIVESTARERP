namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_BrandAdd_Dealer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblDealerSS", "BrandId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblDealerSS", "BrandId");
        }
    }
}
