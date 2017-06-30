namespace JobBoardMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetoSavedJobstable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SavedCompanies", "CompanyCompanyName", c => c.String(maxLength: 128));
            CreateIndex("dbo.SavedCompanies", "CompanyCompanyName");
            AddForeignKey("dbo.SavedCompanies", "CompanyCompanyName", "dbo.Companies", "CompanyName");
            DropColumn("dbo.SavedCompanies", "CompanyName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SavedCompanies", "CompanyName", c => c.String());
            DropForeignKey("dbo.SavedCompanies", "CompanyCompanyName", "dbo.Companies");
            DropIndex("dbo.SavedCompanies", new[] { "CompanyCompanyName" });
            DropColumn("dbo.SavedCompanies", "CompanyCompanyName");
        }
    }
}
