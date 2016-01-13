namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSyncedAtToDriveReport : DbMigration
    {
        public override void Up()
        {
            AddColumn("DriveReports", "SyncedAt", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("DriveReports", "SyncedAt");
        }
    }
}
