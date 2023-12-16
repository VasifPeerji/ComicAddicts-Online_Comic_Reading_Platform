namespace SSL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class merchandiseTableCreated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Merchandise",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 1000),
                        ReleaseDate = c.DateTime(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CategoryId = c.Int(nullable: false),
                        Image = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Merchandise", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Merchandise", new[] { "CategoryId" });
            DropTable("dbo.Merchandise");
        }
    }
}
