namespace JobBoardMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class redofixnamingconventiontomatchCompaniesCompanyNametoJobsCompanyName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "CompanyName", c => c.String());
            AddColumn("dbo.Jobs", "CompanyName", c => c.String());
            DropColumn("dbo.Companies", "Name");
            DropColumn("dbo.Jobs", "Company");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Jobs", "Company", c => c.String());
            AddColumn("dbo.Companies", "Name", c => c.String());
            DropColumn("dbo.Jobs", "CompanyName");
            DropColumn("dbo.Companies", "CompanyName");
        }
    }
}
