namespace FIT5032_Assignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class image1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "Image_Id", c => c.Int());
            CreateIndex("dbo.Appointments", "Image_Id");
            AddForeignKey("dbo.Appointments", "Image_Id", "dbo.Images", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "Image_Id", "dbo.Images");
            DropIndex("dbo.Appointments", new[] { "Image_Id" });
            DropColumn("dbo.Appointments", "Image_Id");
        }

    }
}
