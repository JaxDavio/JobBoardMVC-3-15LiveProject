namespace JobBoardMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Apr17Soltz : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SavedJobs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ApplicationLink = c.String(),
                        Company = c.String(),
                        JobTitle = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SavedJobs");
        }
    }
}
