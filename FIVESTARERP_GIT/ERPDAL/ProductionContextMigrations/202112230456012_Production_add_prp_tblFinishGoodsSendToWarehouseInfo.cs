namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_add_prp_tblFinishGoodsSendToWarehouseInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblFinishGoodsSendToWarehouseInfo", "ColorId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblFinishGoodsSendToWarehouseInfo", "ColorId");
        }
    }
}
