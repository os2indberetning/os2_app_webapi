namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedUnusedFieldsOnRates : DbMigration
    {
        public override void Up()
        {
            AddColumn("Rates", "Description", c => c.String(unicode: false));
            DropColumn("Rates", "KmRate");
            DropColumn("Rates", "TFCode");
            DropColumn("Rates", "Type");
        }
        
        public override void Down()
        {
            AddColumn("Rates", "Type", c => c.String(unicode: false));
            AddColumn("Rates", "TFCode", c => c.String(unicode: false));
            AddColumn("Rates", "KmRate", c => c.String(unicode: false));
            DropColumn("Rates", "Description");
        }
    }
}
