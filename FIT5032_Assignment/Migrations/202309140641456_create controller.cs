namespace FIT5032_Assignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createcontroller : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentId = c.Int(nullable: false, identity: true),
                        PatientName = c.String(nullable: false),
                        Birthday = c.DateTime(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        ScanPart = c.String(nullable: false),
                        Gender = c.String(nullable: false),
                        Note = c.String(maxLength: 500),
                        ClinicId = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                        Status = c.String(nullable: false),
                        UserID = c.String(),
                        Patient_Id = c.Int(),
                    })
                .PrimaryKey(t => t.AppointmentId)
                .ForeignKey("dbo.Clinics", t => t.ClinicId)
                .ForeignKey("dbo.Patients", t => t.Patient_Id)
                .Index(t => t.ClinicId)
                .Index(t => t.Patient_Id);
            
            CreateTable(
                "dbo.Clinics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClinicName = c.String(nullable: false, maxLength: 255),
                        Address = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        ScheduleId = c.Int(nullable: false, identity: true),
                        TimeSlot = c.DateTime(nullable: false),
                        IsOccupied = c.Boolean(nullable: false),
                        ClinicId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ScheduleId)
                .ForeignKey("dbo.Clinics", t => t.ClinicId)
                .Index(t => t.ClinicId);
            
            CreateTable(
                "dbo.FeedbackAndRatings",
                c => new
                    {
                        AppointmentId = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        Comment = c.String(nullable: false, maxLength: 1000),
                    })
                .PrimaryKey(t => t.AppointmentId)
                .ForeignKey("dbo.Appointments", t => t.AppointmentId)
                .Index(t => t.AppointmentId);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "Patient_Id", "dbo.Patients");
            DropForeignKey("dbo.FeedbackAndRatings", "AppointmentId", "dbo.Appointments");
            DropForeignKey("dbo.Appointments", "ClinicId", "dbo.Clinics");
            DropForeignKey("dbo.Schedules", "ClinicId", "dbo.Clinics");
            DropIndex("dbo.FeedbackAndRatings", new[] { "AppointmentId" });
            DropIndex("dbo.Schedules", new[] { "ClinicId" });
            DropIndex("dbo.Appointments", new[] { "Patient_Id" });
            DropIndex("dbo.Appointments", new[] { "ClinicId" });
            DropTable("dbo.Patients");
            DropTable("dbo.FeedbackAndRatings");
            DropTable("dbo.Schedules");
            DropTable("dbo.Clinics");
            DropTable("dbo.Appointments");
        }
    }
}
