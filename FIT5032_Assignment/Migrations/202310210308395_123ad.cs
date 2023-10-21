namespace FIT5032_Assignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _123ad : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Fora", newName: "ForumPosts");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ForumPosts", newName: "Fora");
        }
    }
}
