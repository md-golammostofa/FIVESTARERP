namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_add_prp_tblStockItemReturnDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblStockItemReturnDetail", "GoodStockQty", c => c.Int(nullable: false));
            AddColumn("dbo.tblStockItemReturnDetail", "ManMadeFaultyQty", c => c.Int(nullable: false));
            AddColumn("dbo.tblStockItemReturnDetail", "ChinaFaultyQty", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblStockItemReturnDetail", "ChinaFaultyQty");
            DropColumn("dbo.tblStockItemReturnDetail", "ManMadeFaultyQty");
            DropColumn("dbo.tblStockItemReturnDetail", "GoodStockQty");
        }
    }
}
