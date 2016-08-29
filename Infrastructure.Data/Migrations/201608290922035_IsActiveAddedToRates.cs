namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsActiveAddedToRates : DbMigration
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
