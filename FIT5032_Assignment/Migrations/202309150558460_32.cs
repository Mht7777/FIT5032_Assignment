namespace FIT5032_Assignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _32 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Appointments", "Patient_Id", "dbo.Patients");
            DropIndex("dbo.Appointments", new[] { "Patient_Id" });
            RenameColumn(table: "dbo.Appointments", name: "Patient_Id", newName: "PatientId");
            AddColumn("dbo.Appointments", "IsConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Schedules", "AppointmentId", c => c.Int());
            AddColumn("dbo.Patients", "PatientName", c => c.String(nullable: false));
            AddColumn("dbo.Patients", "Birthday", c => c.DateTime(nullable: false));
            AddColumn("dbo.Patients", "PhoneNumber", c => c.String(nullable: false));
            AddColumn("dbo.Patients", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Patients", "Gender", c => c.String(nullable: false));
            AlterColumn("dbo.Appointments", "PatientId", c => c.Int(nullable: false));
            CreateIndex("dbo.Appointments", "PatientId");
            CreateIndex("dbo.Schedules", "AppointmentId");
            AddForeignKey("dbo.Schedules", "AppointmentId", "dbo.Appointments", "AppointmentId");
            AddForeignKey("dbo.Appointments", "PatientId", "dbo.Patients", "Id", cascadeDelete: true);
            DropColumn("dbo.Appointments", "PatientName");
            DropColumn("dbo.Appointments", "Birthday");
            DropColumn("dbo.Appointments", "PhoneNumber");
            DropColumn("dbo.Appointments", "Email");
            DropColumn("dbo.Appointments", "Gender");
            DropColumn("dbo.Appointments", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appointments", "Status", c => c.String(nullable: false));
            AddColumn("dbo.Appointments", "Gender", c => c.String(nullable: false));
            AddColumn("dbo.Appointments", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Appointments", "PhoneNumber", c => c.String(nullable: false));
            AddColumn("dbo.Appointments", "Birthday", c => c.DateTime(nullable: false));
            AddColumn("dbo.Appointments", "PatientName", c => c.String(nullable: false));
            DropForeignKey("dbo.Appointments", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.Schedules", "AppointmentId", "dbo.Appointments");
            DropIndex("dbo.Schedules", new[] { "AppointmentId" });
            DropIndex("dbo.Appointments", new[] { "PatientId" });
            AlterColumn("dbo.Appointments", "PatientId", c => c.Int());
            DropColumn("dbo.Patients", "Gender");
            DropColumn("dbo.Patients", "Email");
            DropColumn("dbo.Patients", "PhoneNumber");
            DropColumn("dbo.Patients", "Birthday");
            DropColumn("dbo.Patients", "PatientName");
            DropColumn("dbo.Schedules", "AppointmentId");
            DropColumn("dbo.Appointments", "IsConfirmed");
            RenameColumn(table: "dbo.Appointments", name: "PatientId", newName: "Patient_Id");
            CreateIndex("dbo.Appointments", "Patient_Id");
            AddForeignKey("dbo.Appointments", "Patient_Id", "dbo.Patients", "Id");
        }
    }
}
