namespace SSL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FreeTrialsCreated2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FreeTrials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Trials = c.Int(nullable: false),
                        Downloads = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FreeTrials", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.FreeTrials", new[] { "UserId" });
            DropTable("dbo.FreeTrials");
        }
    }
}
