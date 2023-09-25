namespace FIT5032_Assignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intergratt_time_2_appointmnet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "AppointmentDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Appointments", "StartTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Appointments", "EndTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Appointments", "AppointmentDateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appointments", "AppointmentDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Appointments", "EndTime");
            DropColumn("dbo.Appointments", "StartTime");
            DropColumn("dbo.Appointments", "AppointmentDate");
        }
    }
}
