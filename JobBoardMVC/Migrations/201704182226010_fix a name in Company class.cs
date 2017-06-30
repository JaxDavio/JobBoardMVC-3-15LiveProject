namespace JobBoardMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixanameinCompanyclass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "CompanyLink", c => c.String());
            DropColumn("dbo.Companies", "ApplicationLink");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "ApplicationLink", c => c.String());
            DropColumn("dbo.Companies", "CompanyLink");
        }
    }
}
