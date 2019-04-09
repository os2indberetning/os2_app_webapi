namespace Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveToken : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Tokens", "ProfileId", "Profiles");
            DropIndex("Tokens", new[] { "ProfileId" });
            DropTable("Tokens");
        }
        
        public override void Down()
        {
            CreateTable(
                "Tokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GuId = c.String(unicode: false),
                        TokenString = c.String(unicode: false),
                        Status = c.Int(nullable: false),
                        ProfileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)                ;
            
            CreateIndex("Tokens", "ProfileId");
            AddForeignKey("Tokens", "ProfileId", "Profiles", "Id", cascadeDelete: true);
        }
    }
}
