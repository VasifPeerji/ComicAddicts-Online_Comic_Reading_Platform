namespace SSL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class freeTrialsTableDeleted : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FreeTrials", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.FreeTrials", new[] { "UserId" });
            DropTable("dbo.FreeTrials");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.FreeTrials", "UserId");
            AddForeignKey("dbo.FreeTrials", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
