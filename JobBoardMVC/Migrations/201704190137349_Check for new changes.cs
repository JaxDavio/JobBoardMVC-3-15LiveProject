namespace JobBoardMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Checkfornewchanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "Name", c => c.String());
            AddColumn("dbo.Jobs", "Company", c => c.String());
            DropColumn("dbo.Companies", "CompanyName");
            DropColumn("dbo.Jobs", "CompanyName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Jobs", "CompanyName", c => c.String());
            AddColumn("dbo.Companies", "CompanyName", c => c.String());
            DropColumn("dbo.Jobs", "Company");
            DropColumn("dbo.Companies", "Name");
        }
    }
}
