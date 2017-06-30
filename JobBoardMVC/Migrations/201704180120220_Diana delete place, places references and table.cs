namespace JobBoardMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dianadeleteplaceplacesreferencesandtable : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Places");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Places",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
    }
}
