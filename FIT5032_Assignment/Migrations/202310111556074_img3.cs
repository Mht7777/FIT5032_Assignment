namespace FIT5032_Assignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class img3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Appointments", "Image_Id", "dbo.Images");
            DropIndex("dbo.Appointments", new[] { "Image_Id" });
            AddColumn("dbo.Appointments", "Image_Id1", c => c.Int());
            CreateIndex("dbo.Appointments", "Image_Id1");
            AddForeignKey("dbo.Appointments", "Image_Id1", "dbo.Images", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "Image_Id1", "dbo.Images");
            DropIndex("dbo.Appointments", new[] { "Image_Id1" });
            DropColumn("dbo.Appointments", "Image_Id1");
            CreateIndex("dbo.Appointments", "Image_Id");
            AddForeignKey("dbo.Appointments", "Image_Id", "dbo.Images", "Id");
        }
    }
}
