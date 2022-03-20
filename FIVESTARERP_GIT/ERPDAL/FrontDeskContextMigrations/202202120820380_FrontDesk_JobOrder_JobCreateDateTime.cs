namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_JobOrder_JobCreateDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "JobCreateDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "JobCreateDateTime");
        }
    }
}
