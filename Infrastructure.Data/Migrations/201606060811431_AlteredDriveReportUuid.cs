namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlteredDriveReportUuid : DbMigration
    {
        public override void Up()
        {
            AlterColumn("DriveReports", "Uuid", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("DriveReports", "Uuid", c => c.Guid(nullable: false));
        }
    }
}
