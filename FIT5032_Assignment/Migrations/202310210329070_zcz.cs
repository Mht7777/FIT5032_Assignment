namespace FIT5032_Assignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zcz : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ForumPosts", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.ForumPosts", "Content", c => c.String(nullable: false));
            AlterColumn("dbo.ForumPosts", "Author", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ForumPosts", "Author", c => c.String());
            AlterColumn("dbo.ForumPosts", "Content", c => c.String());
            AlterColumn("dbo.ForumPosts", "Title", c => c.String());
        }
    }
}
