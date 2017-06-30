namespace JobBoardMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddnewSavedJobstable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SavedJobs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Guid(nullable: false),
                        ApplicationLink = c.String(),
                        CompanyCompanyName = c.String(maxLength: 128),
                        JobTitle = c.String(),
                        Location = c.String(),
                        DatePosted = c.String(),
                        DateSaved = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Companies", t => t.CompanyCompanyName)
                .Index(t => t.CompanyCompanyName);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SavedJobs", "CompanyCompanyName", "dbo.Companies");
            DropIndex("dbo.SavedJobs", new[] { "CompanyCompanyName" });
            DropTable("dbo.SavedJobs");
        }
    }
}
