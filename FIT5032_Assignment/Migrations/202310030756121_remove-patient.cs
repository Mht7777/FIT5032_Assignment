namespace FIT5032_Assignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removepatient : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Appointments", "PatientId", "dbo.Patients");
            DropIndex("dbo.Appointments", new[] { "PatientId" });
            AddColumn("dbo.Appointments", "FirstName", c => c.String(nullable: false));
            AddColumn("dbo.Appointments", "LastName", c => c.String(nullable: false));
            AddColumn("dbo.Appointments", "Birthday", c => c.DateTime(nullable: false));
            AddColumn("dbo.Appointments", "PhoneNumber", c => c.String(nullable: false));
            AddColumn("dbo.Appointments", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Appointments", "Gender", c => c.String(nullable: false));
            DropColumn("dbo.Appointments", "PatientId");
            DropTable("dbo.Patients");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        PatientName = c.String(nullable: false),
                        Birthday = c.DateTime(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Gender = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Appointments", "PatientId", c => c.Int(nullable: false));
            DropColumn("dbo.Appointments", "Gender");
            DropColumn("dbo.Appointments", "Email");
            DropColumn("dbo.Appointments", "PhoneNumber");
            DropColumn("dbo.Appointments", "Birthday");
            DropColumn("dbo.Appointments", "LastName");
            DropColumn("dbo.Appointments", "FirstName");
            CreateIndex("dbo.Appointments", "PatientId");
            AddForeignKey("dbo.Appointments", "PatientId", "dbo.Patients", "Id", cascadeDelete: true);
        }
    }
}
