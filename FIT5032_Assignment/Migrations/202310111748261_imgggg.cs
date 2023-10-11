namespace FIT5032_Assignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imgggg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Images", "AppointmentId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Images", "AppointmentId");
        }
    }
}
