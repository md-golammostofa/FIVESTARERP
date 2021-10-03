namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_SubQC : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblSubQC",
                c => new
                    {
                        SubQCId = c.Long(nullable: false, identity: true),
                        SubQCName = c.String(),
                        QCId = c.Long(nullable: false),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SubQCId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblSubQC");
        }
    }
}
