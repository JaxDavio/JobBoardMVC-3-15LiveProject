namespace JobBoardMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyFiles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyFiles",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 255),
                        ContentType = c.String(maxLength: 100),
                        Content = c.Binary(),
                        FileType = c.Int(nullable: false),
                        CompanyName = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FileId)
                .ForeignKey("dbo.Companies", t => t.CompanyName)
                .Index(t => t.CompanyName);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyFiles", "CompanyName", "dbo.Companies");
            DropIndex("dbo.CompanyFiles", new[] { "CompanyName" });
            DropTable("dbo.CompanyFiles");
        }
    }
}
