namespace JobBoardMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add2columnstoCompanyTableforeasierscraping : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "JobCode", c => c.String());
            AddColumn("dbo.Companies", "LocationCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "LocationCode");
            DropColumn("dbo.Companies", "JobCode");
        }
    }
}
