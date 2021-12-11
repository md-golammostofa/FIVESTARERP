namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_add_prp_tblFinishGoodSendToWarehouseDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblFinishGoodsSendToWarehouseDetail", "PackagingLineId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblFinishGoodsSendToWarehouseDetail", "PackagingLineId");
        }
    }
}
