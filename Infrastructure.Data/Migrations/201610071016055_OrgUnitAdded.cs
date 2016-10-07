namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrgUnitAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "OrgUnits",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        OrgId = c.Int(nullable: false),
                        FourKmRuleAllowed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)                ;
            Sql("INSERT INTO OrgUnits VALUES(1,1,0);"); //Insert dummy orgunit to handle default values in new column on Employment until real data is added when DMZSync runs.
            AddColumn("DriveReports", "HomeToBorderDistance", c => c.Double(nullable: false, defaultValueSql: "1"));
            AddColumn("Employments", "OrgUnitId", c => c.Int(nullable: false));
            AddColumn("Rates", "isActive", c => c.Boolean(nullable: false));
            CreateIndex("Employments", "OrgUnitId");
            AddForeignKey("Employments", "OrgUnitId", "OrgUnits", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Employments", "OrgUnitId", "OrgUnits");
            DropIndex("Employments", new[] { "OrgUnitId" });
            DropColumn("Rates", "isActive");
            DropColumn("Employments", "OrgUnitId");
            DropColumn("DriveReports", "HomeToBorderDistance");
            DropTable("OrgUnits");
        }
    }
}
