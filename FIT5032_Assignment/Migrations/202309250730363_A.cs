namespace FIT5032_Assignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class A : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schedules", "StartTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Schedules", "EndTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Schedules", "TimeSlot");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schedules", "TimeSlot", c => c.DateTime(nullable: false));
            DropColumn("dbo.Schedules", "EndTime");
            DropColumn("dbo.Schedules", "StartTime");
        }
    }
}
