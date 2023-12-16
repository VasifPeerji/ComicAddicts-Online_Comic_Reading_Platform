namespace SSL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RatingsTableCreated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ComicId = c.Int(nullable: false),
                        MerchandiseId = c.Int(nullable: false),
                        Stars = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comics", t => t.ComicId, cascadeDelete: true)
                .ForeignKey("dbo.Merchandise", t => t.MerchandiseId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.ComicId)
                .Index(t => t.MerchandiseId)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ratings", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ratings", "MerchandiseId", "dbo.Merchandise");
            DropForeignKey("dbo.Ratings", "ComicId", "dbo.Comics");
            DropIndex("dbo.Ratings", new[] { "User_Id" });
            DropIndex("dbo.Ratings", new[] { "MerchandiseId" });
            DropIndex("dbo.Ratings", new[] { "ComicId" });
            DropTable("dbo.Ratings");
        }
    }
}
