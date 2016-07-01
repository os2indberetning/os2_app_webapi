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
                        Id = c.Int(nullable: false, identity: true),
                        OrgId = c.Int(nullable: false),
                        FourKmRuleAllowed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)                ;
            
            AddColumn("Employments", "OrgUnitId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Employments", "OrgUnitId");
            DropTable("OrgUnits");
        }
    }
}
