namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_MUserId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "MUserId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "MUserId");
        }
    }
}
