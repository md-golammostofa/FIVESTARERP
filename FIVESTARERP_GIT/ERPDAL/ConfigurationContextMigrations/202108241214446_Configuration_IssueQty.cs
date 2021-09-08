namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_IssueQty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTransferDetails", "IssueQty", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTransferDetails", "IssueQty");
        }
    }
}
