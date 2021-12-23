namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_QCandStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "TotalPOrDStatus", c => c.String());
            AddColumn("dbo.tblJobOrders", "QCName", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "QCName");
            DropColumn("dbo.tblJobOrders", "TotalPOrDStatus");
        }
    }
}
