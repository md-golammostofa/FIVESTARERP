namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_Requsition_IssueQty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblRequsitionDetailForJobOrders", "IssueQty", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblRequsitionDetailForJobOrders", "IssueQty");
        }
    }
}
