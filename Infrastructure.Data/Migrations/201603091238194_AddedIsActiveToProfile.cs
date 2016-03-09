namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIsActiveToProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("Profiles", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Profiles", "IsActive");
        }
    }
}
