namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIsViaPointToGPSCoordinate : DbMigration
    {
        public override void Up()
        {
            AddColumn("GPSCoordinates", "IsViaPoint", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("GPSCoordinates", "IsViaPoint");
        }
    }
}
