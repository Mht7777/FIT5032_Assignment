namespace FIT5032_Assignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_staff : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Staffs",
                c => new
                    {
                        StaffId = c.Int(nullable: false, identity: true),
                        ClinicId = c.Int(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.StaffId)
                .ForeignKey("dbo.Clinics", t => t.ClinicId, cascadeDelete: true)
                .Index(t => t.ClinicId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Staffs", "ClinicId", "dbo.Clinics");
            DropIndex("dbo.Staffs", new[] { "ClinicId" });
            DropTable("dbo.Staffs");
        }
    }
}
