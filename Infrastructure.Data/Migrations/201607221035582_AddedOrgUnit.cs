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
                        Id = c.Int(nullable: false, identity: true),
                        OrgId = c.Int(nullable: false),
                        FourKmRuleAllowed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)                ;
            
            AddColumn("Employments", "OrgUnitId", c => c.Int());
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
