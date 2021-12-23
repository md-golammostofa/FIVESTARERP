namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_ReqIssueUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRequsitionInfoForJobOrders", "IssueUserId", c => c.Long());
            AddColumn("dbo.tblRequsitionInfoForJobOrders", "IssueDateDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRequsitionInfoForJobOrders", "IssueDateDate");
            DropColumn("dbo.tblRequsitionInfoForJobOrders", "IssueUserId");
        }
    }
}
