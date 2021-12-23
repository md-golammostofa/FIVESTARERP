namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_QCAssignDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "QCAssignDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "QCAssignDate");
        }
    }
}
