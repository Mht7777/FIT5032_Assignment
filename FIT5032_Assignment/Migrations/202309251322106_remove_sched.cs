namespace FIT5032_Assignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_sched : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Schedules", "AppointmentId", "dbo.Appointments");
            DropForeignKey("dbo.Schedules", "ClinicId", "dbo.Clinics");
            DropIndex("dbo.Schedules", new[] { "ClinicId" });
            DropIndex("dbo.Schedules", new[] { "AppointmentId" });
            AddColumn("dbo.Appointments", "AppointmentDateTime", c => c.DateTime(nullable: false));
            DropTable("dbo.Schedules");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        ScheduleId = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        IsOccupied = c.Boolean(nullable: false),
                        ClinicId = c.Int(nullable: false),
                        AppointmentId = c.Int(),
                    })
                .PrimaryKey(t => t.ScheduleId);
            
            DropColumn("dbo.Appointments", "AppointmentDateTime");
            CreateIndex("dbo.Schedules", "AppointmentId");
            CreateIndex("dbo.Schedules", "ClinicId");
            AddForeignKey("dbo.Schedules", "ClinicId", "dbo.Clinics", "Id");
            AddForeignKey("dbo.Schedules", "AppointmentId", "dbo.Appointments", "AppointmentId");
        }
    }
}
