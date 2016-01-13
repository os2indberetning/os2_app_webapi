namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFullNameToProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("Profiles", "FullName", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("Profiles", "FullName");
        }
    }
}
