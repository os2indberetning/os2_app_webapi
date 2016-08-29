namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFourKmRuleToDriveReport : DbMigration
    {
        public override void Up()
        {
            AddColumn("DriveReports", "FourKmRule", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("DriveReports", "FourKmRule");
        }
    }
}
