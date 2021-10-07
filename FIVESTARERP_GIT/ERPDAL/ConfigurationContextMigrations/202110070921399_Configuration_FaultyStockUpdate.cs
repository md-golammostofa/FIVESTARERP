namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_FaultyStockUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tblFaultyStockDetails", "TSId", c => c.Long());
            AlterColumn("dbo.tblFaultyStockDetails", "FaultyStockInfoId", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblFaultyStockDetails", "FaultyStockInfoId", c => c.Long(nullable: false));
            AlterColumn("dbo.tblFaultyStockDetails", "TSId", c => c.Long(nullable: false));
        }
    }
}
