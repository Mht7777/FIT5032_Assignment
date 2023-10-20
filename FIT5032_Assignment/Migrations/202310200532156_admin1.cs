namespace FIT5032_Assignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class admin1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Admins", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Admins", "UserId");
            AddForeignKey("dbo.Admins", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Admins", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Admins", new[] { "UserId" });
            AlterColumn("dbo.Admins", "UserId", c => c.String());
        }
    }
}
