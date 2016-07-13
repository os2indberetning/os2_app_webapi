namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_isActive_To_Rate : DbMigration
    {
        public override void Up()
        {
            AddColumn("Rates", "isActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Rates", "isActive");
        }
    }
}
