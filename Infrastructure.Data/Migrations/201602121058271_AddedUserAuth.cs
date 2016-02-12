namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserAuth : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "UserAuths",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(unicode: false),
                        GuId = c.String(unicode: false),
                        Password = c.String(unicode: false),
                        Salt = c.String(unicode: false),
                        ProfileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)                
                .ForeignKey("Profiles", t => t.ProfileId, cascadeDelete: true)
                .Index(t => t.ProfileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("UserAuths", "ProfileId", "Profiles");
            DropIndex("UserAuths", new[] { "ProfileId" });
            DropTable("UserAuths");
        }
    }
}
