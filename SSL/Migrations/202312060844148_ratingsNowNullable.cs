namespace SSL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ratingsNowNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ratings", "ComicId", "dbo.Comics");
            DropForeignKey("dbo.Ratings", "MerchandiseId", "dbo.Merchandise");
            DropIndex("dbo.Ratings", new[] { "ComicId" });
            DropIndex("dbo.Ratings", new[] { "MerchandiseId" });
            AlterColumn("dbo.Ratings", "ComicId", c => c.Int());
            AlterColumn("dbo.Ratings", "MerchandiseId", c => c.Int());
            CreateIndex("dbo.Ratings", "ComicId");
            CreateIndex("dbo.Ratings", "MerchandiseId");
            AddForeignKey("dbo.Ratings", "ComicId", "dbo.Comics", "Id");
            AddForeignKey("dbo.Ratings", "MerchandiseId", "dbo.Merchandise", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ratings", "MerchandiseId", "dbo.Merchandise");
            DropForeignKey("dbo.Ratings", "ComicId", "dbo.Comics");
            DropIndex("dbo.Ratings", new[] { "MerchandiseId" });
            DropIndex("dbo.Ratings", new[] { "ComicId" });
            AlterColumn("dbo.Ratings", "MerchandiseId", c => c.Int(nullable: false));
            AlterColumn("dbo.Ratings", "ComicId", c => c.Int(nullable: false));
            CreateIndex("dbo.Ratings", "MerchandiseId");
            CreateIndex("dbo.Ratings", "ComicId");
            AddForeignKey("dbo.Ratings", "MerchandiseId", "dbo.Merchandise", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Ratings", "ComicId", "dbo.Comics", "Id", cascadeDelete: true);
        }
    }
}
