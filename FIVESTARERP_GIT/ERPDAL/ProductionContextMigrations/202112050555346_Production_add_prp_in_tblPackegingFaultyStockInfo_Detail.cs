namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_add_prp_in_tblPackegingFaultyStockInfo_Detail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblPackagingFaultyStockDetail", "PackagingFaultyStockInfoId", c => c.Long(nullable: false));
            AddColumn("dbo.tblPackagingFaultyStockInfo", "ChinaMadeFaultyStockInQty", c => c.Int(nullable: false));
            AddColumn("dbo.tblPackagingFaultyStockInfo", "ChinaMadeFaultyStockOutQty", c => c.Int(nullable: false));
            AddColumn("dbo.tblPackagingFaultyStockInfo", "ManMadeFaultyStockInQty", c => c.Int(nullable: false));
            AddColumn("dbo.tblPackagingFaultyStockInfo", "ManMadeFaultyStockOutQty", c => c.Int(nullable: false));
            DropColumn("dbo.tblPackagingFaultyStockInfo", "StockInQty");
            DropColumn("dbo.tblPackagingFaultyStockInfo", "StockOutQty");
            DropColumn("dbo.tblPackagingFaultyStockInfo", "IsChinaFaulty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblPackagingFaultyStockInfo", "IsChinaFaulty", c => c.Boolean(nullable: false));
            AddColumn("dbo.tblPackagingFaultyStockInfo", "StockOutQty", c => c.Int(nullable: false));
            AddColumn("dbo.tblPackagingFaultyStockInfo", "StockInQty", c => c.Int(nullable: false));
            DropColumn("dbo.tblPackagingFaultyStockInfo", "ManMadeFaultyStockOutQty");
            DropColumn("dbo.tblPackagingFaultyStockInfo", "ManMadeFaultyStockInQty");
            DropColumn("dbo.tblPackagingFaultyStockInfo", "ChinaMadeFaultyStockOutQty");
            DropColumn("dbo.tblPackagingFaultyStockInfo", "ChinaMadeFaultyStockInQty");
            DropColumn("dbo.tblPackagingFaultyStockDetail", "PackagingFaultyStockInfoId");
        }
    }
}
