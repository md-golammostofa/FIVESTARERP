namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_IssueAndRecDateTrandferToBranchReq : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTransferInfo", "IssueDate", c => c.DateTime());
            AddColumn("dbo.tblTransferInfo", "ReceivedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTransferInfo", "ReceivedDate");
            DropColumn("dbo.tblTransferInfo", "IssueDate");
        }
    }
}
