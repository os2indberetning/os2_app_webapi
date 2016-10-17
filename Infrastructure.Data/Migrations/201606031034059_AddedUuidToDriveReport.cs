namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUuidToDriveReport : DbMigration
    {
        public override void Up()
        {
            AddColumn("DriveReports", "Uuid", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("DriveReports", "Uuid");
        }
    }
}
