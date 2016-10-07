namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOrgUnit : DbMigration
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
            //TODO: Make dummy orgunit with id = 1
            Sql("INSERT INTO OrgUnits VALUES(1, 1, 0);");
            AddColumn("Employments", "OrgUnitId", c => c.Int(nullable: false, defaultValue: 1)); //TODO: Set default to 1
            CreateIndex("Employments", "OrgUnitId");
            AddForeignKey("Employments", "OrgUnitId", "OrgUnits", "Id", cascadeDelete: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Employments", "OrgUnitId", "OrgUnits");
            DropIndex("Employments", new[] { "OrgUnitId" });
            DropColumn("Employments", "OrgUnitId");
            DropTable("OrgUnits");
        }
    }
}
