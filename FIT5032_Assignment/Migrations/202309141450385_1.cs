namespace FIT5032_Assignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Patients", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Patients", "UserId", c => c.Int(nullable: false));
        }
    }
}
