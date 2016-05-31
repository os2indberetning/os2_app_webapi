namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStartAndEndTimeToEmployment : DbMigration
    {
        public override void Up()
        {
            AddColumn("Employments", "StartDateTimestamp", c => c.Long(nullable: false));
            AddColumn("Employments", "EndDateTimestamp", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Employments", "EndDateTimestamp");
            DropColumn("Employments", "StartDateTimestamp");
        }
    }
}
