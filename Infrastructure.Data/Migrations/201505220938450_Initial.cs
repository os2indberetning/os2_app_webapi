namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DriveReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.String(unicode: false),
                        ManualEntryRemark = c.String(unicode: false),
                        Purpose = c.String(unicode: false),
                        StartsAtHome = c.Boolean(nullable: false),
                        EndsAtHome = c.Boolean(nullable: false),
                        EmploymentId = c.Int(nullable: false),
                        ProfileId = c.Int(nullable: false),
                        RateId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)                
                .ForeignKey("Profiles", t => t.ProfileId, cascadeDelete: true)
                .ForeignKey("Rates", t => t.RateId, cascadeDelete: true)
                .Index(t => t.ProfileId)
                .Index(t => t.RateId);
            
            CreateTable(
                "Profiles",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        FirstName = c.String(unicode: false),
                        LastName = c.String(unicode: false),
                        HomeLatitude = c.String(unicode: false),
                        HomeLongitude = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)                ;
            
            CreateTable(
                "Employments",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        EmploymentPosition = c.String(unicode: false),
                        ManNr = c.String(unicode: false),
                        ProfileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)                
                .ForeignKey("Profiles", t => t.ProfileId, cascadeDelete: true)
                .Index(t => t.ProfileId);
            
            CreateTable(
                "Tokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GuId = c.String(unicode: false),
                        TokenString = c.String(unicode: false),
                        Status = c.Int(nullable: false),
                        ProfileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)                
                .ForeignKey("Profiles", t => t.ProfileId, cascadeDelete: true)
                .Index(t => t.ProfileId);
            
            CreateTable(
                "Rates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KmRate = c.String(unicode: false),
                        TFCode = c.String(unicode: false),
                        Type = c.String(unicode: false),
                        Year = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)                ;
            
            CreateTable(
                "Routes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        TotalDistance = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)                
                .ForeignKey("DriveReports", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "GPSCoordinates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Latitude = c.String(unicode: false),
                        Longitude = c.String(unicode: false),
                        TimeStamp = c.String(unicode: false),
                        RouteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)                
                .ForeignKey("Routes", t => t.RouteId, cascadeDelete: true)
                .Index(t => t.RouteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Routes", "Id", "DriveReports");
            DropForeignKey("GPSCoordinates", "RouteId", "Routes");
            DropForeignKey("DriveReports", "RateId", "Rates");
            DropForeignKey("Tokens", "ProfileId", "Profiles");
            DropForeignKey("Employments", "ProfileId", "Profiles");
            DropForeignKey("DriveReports", "ProfileId", "Profiles");
            DropIndex("GPSCoordinates", new[] { "RouteId" });
            DropIndex("Routes", new[] { "Id" });
            DropIndex("Tokens", new[] { "ProfileId" });
            DropIndex("Employments", new[] { "ProfileId" });
            DropIndex("DriveReports", new[] { "RateId" });
            DropIndex("DriveReports", new[] { "ProfileId" });
            DropTable("GPSCoordinates");
            DropTable("Routes");
            DropTable("Rates");
            DropTable("Tokens");
            DropTable("Employments");
            DropTable("Profiles");
            DropTable("DriveReports");
        }
    }
}
