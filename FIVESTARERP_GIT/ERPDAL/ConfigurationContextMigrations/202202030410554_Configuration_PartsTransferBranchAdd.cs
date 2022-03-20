namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_PartsTransferBranchAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblPartsTransferHToCInfo", "BranchToId", c => c.Long(nullable: false));
            AddColumn("dbo.tblPartsTransferHToCInfo", "BranchFromId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblPartsTransferHToCInfo", "BranchFromId");
            DropColumn("dbo.tblPartsTransferHToCInfo", "BranchToId");
        }
    }
}
