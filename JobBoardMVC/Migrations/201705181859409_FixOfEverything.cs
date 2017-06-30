namespace JobBoardMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixOfEverything : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResumeModels",
                c => new
                    {
                        FileId = c.Guid(nullable: false),
                        FileName = c.String(maxLength: 255),
                        ContentType = c.String(maxLength: 100),
                        Content = c.Binary(),
                        FileType = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FileId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ResumeModels", "UserId", "dbo.User");
            DropIndex("dbo.ResumeModels", new[] { "UserId" });
            DropTable("dbo.ResumeModels");
        }
    }
}
