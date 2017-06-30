namespace JobBoardMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixCompanytemporarilyremovedsecondattempt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "CompanyCompanyName", c => c.String());
            DropColumn("dbo.Jobs", "CompanyName");
            DropTable("dbo.Companies");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CompanyLink = c.String(),
                        CompanyName = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        CareerPage = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Jobs", "CompanyName", c => c.String());
            DropColumn("dbo.Jobs", "CompanyCompanyName");
        }
    }
}
