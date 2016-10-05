namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlteredOrgUnitIdOnEmployment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Employments", "OrgUnitId", "OrgUnits");
            DropIndex("Employments", new[] { "OrgUnitId" });
            AlterColumn("Employments", "OrgUnitId", c => c.Int());
            CreateIndex("Employments", "OrgUnitId");
            AddForeignKey("Employments", "OrgUnitId", "OrgUnits", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Employments", "OrgUnitId", "OrgUnits");
            DropIndex("Employments", new[] { "OrgUnitId" });
            AlterColumn("Employments", "OrgUnitId", c => c.Int(nullable: false));
            CreateIndex("Employments", "OrgUnitId");
            AddForeignKey("Employments", "OrgUnitId", "OrgUnits", "Id", cascadeDelete: true);
        }
    }
}
