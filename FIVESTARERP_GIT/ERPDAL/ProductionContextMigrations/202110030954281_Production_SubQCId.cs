namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_SubQCId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblQRCodeProblem", "SubQCId", c => c.Long());
            AddColumn("dbo.tblQRCodeTransferToRepairInfo", "SubQCId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblQRCodeTransferToRepairInfo", "SubQCId");
            DropColumn("dbo.tblQRCodeProblem", "SubQCId");
        }
    }
}
