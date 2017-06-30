namespace JobBoardMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedcontrollersandmodelsforresumeuploadfeature : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Resume", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "Resume");
        }
    }
}
