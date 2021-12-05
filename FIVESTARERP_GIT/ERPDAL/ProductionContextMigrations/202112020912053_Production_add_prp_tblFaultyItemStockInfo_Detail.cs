namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_add_prp_tblFaultyItemStockInfo_Detail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblFaultyItemStockDetail", "FaultyItemStockInfoId", c => c.Long(nullable: false));
            AddColumn("dbo.tblFaultyItemStockInfo", "ChinaMadeFaultyStockInQty", c => c.Int(nullable: false));
            AddColumn("dbo.tblFaultyItemStockInfo", "ChinaMadeFaultyStockOutQty", c => c.Int(nullable: false));
            AddColumn("dbo.tblFaultyItemStockInfo", "ManMadeFaultyStockInQty", c => c.Int(nullable: false));
            AddColumn("dbo.tblFaultyItemStockInfo", "ManMadeFaultyStockOutQty", c => c.Int(nullable: false));
            AlterColumn("dbo.tblFaultyItemStockInfo", "Remarks", c => c.String());
            DropColumn("dbo.tblFaultyItemStockInfo", "StockInQty");
            DropColumn("dbo.tblFaultyItemStockInfo", "StockOutQty");
            DropColumn("dbo.tblFaultyItemStockInfo", "IsChinaFaulty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblFaultyItemStockInfo", "IsChinaFaulty", c => c.Boolean(nullable: false));
            AddColumn("dbo.tblFaultyItemStockInfo", "StockOutQty", c => c.Int(nullable: false));
            AddColumn("dbo.tblFaultyItemStockInfo", "StockInQty", c => c.Int(nullable: false));
            AlterColumn("dbo.tblFaultyItemStockInfo", "Remarks", c => c.String(maxLength: 150));
            DropColumn("dbo.tblFaultyItemStockInfo", "ManMadeFaultyStockOutQty");
            DropColumn("dbo.tblFaultyItemStockInfo", "ManMadeFaultyStockInQty");
            DropColumn("dbo.tblFaultyItemStockInfo", "ChinaMadeFaultyStockOutQty");
            DropColumn("dbo.tblFaultyItemStockInfo", "ChinaMadeFaultyStockInQty");
            DropColumn("dbo.tblFaultyItemStockDetail", "FaultyItemStockInfoId");
        }
    }
}
