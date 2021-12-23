namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_CallCenterAssignDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "CallCenterAssignDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "CallCenterAssignDate");
        }
    }
}
