namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_add_tblQC2InfoANDtblQC2Detail_tblQC3InfoANDtblQC3Detail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblQC2Detail",
                c => new
                    {
                        QC2DetailId = c.Long(nullable: false, identity: true),
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
                        QC2InfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.QC2DetailId)
                .ForeignKey("dbo.tblQC2Info", t => t.QC2InfoId, cascadeDelete: true)
                .Index(t => t.QC2InfoId);
            
            CreateTable(
                "dbo.tblQC2Info",
                c => new
                    {
                        QC2InfoId = c.Long(nullable: false, identity: true),
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
                .PrimaryKey(t => t.QC2InfoId);
            
            CreateTable(
                "dbo.tblQC3Detail",
                c => new
                    {
                        QC3DetailId = c.Long(nullable: false, identity: true),
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
                        QC3InfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.QC3DetailId)
                .ForeignKey("dbo.tblQC3Info", t => t.QC3InfoId, cascadeDelete: true)
                .Index(t => t.QC3InfoId);
            
            CreateTable(
                "dbo.tblQC3Info",
                c => new
                    {
                        QC3InfoId = c.Long(nullable: false, identity: true),
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
                .PrimaryKey(t => t.QC3InfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblQC3Detail", "QC3InfoId", "dbo.tblQC3Info");
            DropForeignKey("dbo.tblQC2Detail", "QC2InfoId", "dbo.tblQC2Info");
            DropIndex("dbo.tblQC3Detail", new[] { "QC3InfoId" });
            DropIndex("dbo.tblQC2Detail", new[] { "QC2InfoId" });
            DropTable("dbo.tblQC3Info");
            DropTable("dbo.tblQC3Detail");
            DropTable("dbo.tblQC2Info");
            DropTable("dbo.tblQC2Detail");
        }
    }
}
