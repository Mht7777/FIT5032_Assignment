namespace FIT5032_Assignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fhj : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.ForumPosts");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ForumPosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        Author = c.String(nullable: false),
                        PostedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
