namespace JobBoardMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProfilePhotoes", "UserId", "dbo.User");
            DropIndex("dbo.ProfilePhotoes", new[] { "UserId" });
            DropTable("dbo.ProfilePhotoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProfilePhotoes",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 255),
                        ContentType = c.String(maxLength: 100),
                        Content = c.Binary(),
                        FileType = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FileId);
            
            CreateIndex("dbo.ProfilePhotoes", "UserId");
            AddForeignKey("dbo.ProfilePhotoes", "UserId", "dbo.User", "UserId");
        }
    }
}
