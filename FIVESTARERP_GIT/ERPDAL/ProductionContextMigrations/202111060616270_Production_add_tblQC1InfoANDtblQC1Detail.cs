namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_add_tblQC1InfoANDtblQC1Detail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblQC1Detail",
                c => new
                    {
                        QC1DetailId = c.Long(nullable: false, identity: true),
                        TransferId = c.Long(nullable: false),
                        TransferCode = c.String(),
                        FloorId = c.Long(nullable: false),
                        AssemblyLineId = c.Long(nullable: false),
                        QCLineId = c.Long(nullable: false),
                        SubQCId = c.Long(),
                        RepairLineId = c.Long(nullable: false),
                        QRCode = c.String(),
                        DescriptionId = c.Long(nullable: false),
                        ProblemId = c.Long(nullable: false),
                        ProblemName = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        QC1InfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.QC1DetailId)
                .ForeignKey("dbo.tblQC1Info", t => t.QC1InfoId, cascadeDelete: true)
                .Index(t => t.QC1InfoId);
            
            CreateTable(
                "dbo.tblQC1Info",
                c => new
                    {
                        QC1InfoId = c.Long(nullable: false, identity: true),
                        TransferId = c.Long(),
                        TransferCode = c.String(),
                        FloorId = c.Long(nullable: false),
                        AssemblyLineId = c.Long(nullable: false),
                        QCLineId = c.Long(nullable: false),
                        SubQCId = c.Long(),
                        RepairLineId = c.Long(nullable: false),
                        QRCode = c.String(),
                        DescriptionId = c.Long(nullable: false),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        StateStatus = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.QC1InfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblQC1Detail", "QC1InfoId", "dbo.tblQC1Info");
            DropIndex("dbo.tblQC1Detail", new[] { "QC1InfoId" });
            DropTable("dbo.tblQC1Info");
            DropTable("dbo.tblQC1Detail");
        }
    }
}
