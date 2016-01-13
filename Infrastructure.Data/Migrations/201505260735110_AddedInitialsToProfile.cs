namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedInitialsToProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("Profiles", "Initials", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("Profiles", "Initials");
        }
    }
}
