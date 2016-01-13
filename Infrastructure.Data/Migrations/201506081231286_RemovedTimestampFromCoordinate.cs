namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedTimestampFromCoordinate : DbMigration
    {
        public override void Up()
        {
            DropColumn("GPSCoordinates", "TimeStamp");
        }
        
        public override void Down()
        {
            AddColumn("GPSCoordinates", "TimeStamp", c => c.String(unicode: false));
        }
    }
}
