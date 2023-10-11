namespace FIT5032_Assignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_image : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Appointments", "Images_Id", c => c.Int());
            CreateIndex("dbo.Appointments", "Images_Id");
            AddForeignKey("dbo.Appointments", "Images_Id", "dbo.Images", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "Images_Id", "dbo.Images");
            DropIndex("dbo.Appointments", new[] { "Images_Id" });
            DropColumn("dbo.Appointments", "Images_Id");
            DropTable("dbo.Images");
        }
    }
}
