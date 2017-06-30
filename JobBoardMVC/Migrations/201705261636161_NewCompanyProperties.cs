namespace JobBoardMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewCompanyProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "CompanyType", c => c.String());
            AddColumn("dbo.Companies", "IndustryServed", c => c.String());
            AddColumn("dbo.Companies", "LanguagesUsed", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "LanguagesUsed");
            DropColumn("dbo.Companies", "IndustryServed");
            DropColumn("dbo.Companies", "CompanyType");
        }
    }
}
