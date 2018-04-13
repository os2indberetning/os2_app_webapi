namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUniqueIndexToDriveReport : DbMigration
    {
        public override void Up()
        {
            AlterColumn("DriveReports", "Uuid", c => c.String(maxLength: 38, unicode: false));
            CreateIndex("DriveReports", "Uuid", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("DriveReports", new[] { "Uuid" });
            AlterColumn("DriveReports", "Uuid", c => c.String(unicode: false));
        }
    }
}
