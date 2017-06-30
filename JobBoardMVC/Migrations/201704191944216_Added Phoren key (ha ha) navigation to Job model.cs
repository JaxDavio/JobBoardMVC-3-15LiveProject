namespace JobBoardMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPhorenkeyhahanavigationtoJobmodel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Jobs", "CompanyCompanyName", c => c.String(maxLength: 128));
            CreateIndex("dbo.Jobs", "CompanyCompanyName");
            AddForeignKey("dbo.Jobs", "CompanyCompanyName", "dbo.Companies", "CompanyName");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jobs", "CompanyCompanyName", "dbo.Companies");
            DropIndex("dbo.Jobs", new[] { "CompanyCompanyName" });
            AlterColumn("dbo.Jobs", "CompanyCompanyName", c => c.String());
        }
    }
}
