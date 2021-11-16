namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_MiniStockQCLineAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblMiniStockTransferToWarehouseDetails", "QCLine", c => c.Long());
            AddColumn("dbo.tblMiniStockTransferToWarehouseInfo", "QCLine", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblMiniStockTransferToWarehouseInfo", "QCLine");
            DropColumn("dbo.tblMiniStockTransferToWarehouseDetails", "QCLine");
        }
    }
}
