namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_FiveStarSMSDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblFiveStarSMSDetails",
                c => new
                    {
                        SmsId = c.Long(nullable: false, identity: true),
                        MobileNo = c.String(),
                        Message = c.String(),
                        Purpose = c.String(),
                        Response = c.String(),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(),
                        EUserId = c.Long(),
                        UpUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SmsId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblFiveStarSMSDetails");
        }
    }
}
