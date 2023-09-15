namespace FIT5032_Assignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NameOfYourMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FeedbackAndRatings", "Comment", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FeedbackAndRatings", "Comment", c => c.String(nullable: false, maxLength: 1000));
        }
    }
}
