namespace JobBoardMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDescriptionandCatefories : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "Description", c => c.String());
            AddColumn("dbo.Companies", "Categories", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "Categories");
            DropColumn("dbo.Companies", "Description");
        }
    }
}
