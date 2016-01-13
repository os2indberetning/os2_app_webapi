namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GPSCoordinateChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("GPSCoordinates", "IsViaPoint", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("GPSCoordinates", "IsViaPoint", c => c.String(unicode: false));
        }
    }
}
