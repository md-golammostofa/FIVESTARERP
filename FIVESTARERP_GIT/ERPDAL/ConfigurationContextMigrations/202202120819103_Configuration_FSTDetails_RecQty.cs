namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_FSTDetails_RecQty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblFaultyStockTransferDetails", "ReceiveQty", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblFaultyStockTransferDetails", "ReceiveQty");
        }
    }
}
