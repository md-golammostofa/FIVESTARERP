namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_ProbalyDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblJobOrders", "ProbablyDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblJobOrders", "ProbablyDate");
        }
    }
}
