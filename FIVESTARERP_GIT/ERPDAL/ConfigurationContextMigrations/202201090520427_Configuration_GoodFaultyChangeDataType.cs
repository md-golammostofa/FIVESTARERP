namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_GoodFaultyChangeDataType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tblGoodToFaultyTransferInfo", "TransferCode", c => c.String());
            AlterColumn("dbo.tblGoodToFaultyTransferInfo", "TransferStatus", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblGoodToFaultyTransferInfo", "TransferStatus", c => c.Long(nullable: false));
            AlterColumn("dbo.tblGoodToFaultyTransferInfo", "TransferCode", c => c.Long(nullable: false));
        }
    }
}
