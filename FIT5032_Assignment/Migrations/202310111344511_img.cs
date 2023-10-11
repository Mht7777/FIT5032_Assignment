namespace FIT5032_Assignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class img : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Appointments", name: "Images_Id", newName: "Image_Id");
            RenameIndex(table: "dbo.Appointments", name: "IX_Images_Id", newName: "IX_Image_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Appointments", name: "IX_Image_Id", newName: "IX_Images_Id");
            RenameColumn(table: "dbo.Appointments", name: "Image_Id", newName: "Images_Id");
        }
    }
}
