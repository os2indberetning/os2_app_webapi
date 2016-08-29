namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HomeToBorderDistanceAddedToDriveReport : DbMigration
    {
        public override void Up()
        {
            AddColumn("DriveReports", "HomeToBorderDistance", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("DriveReports", "HomeToBorderDistance");
        }
    }
}
