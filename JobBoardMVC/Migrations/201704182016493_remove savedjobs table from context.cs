namespace JobBoardMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removesavedjobstablefromcontext : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.SavedJobs");
        }
        
        public override void Down()
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
    }
}
