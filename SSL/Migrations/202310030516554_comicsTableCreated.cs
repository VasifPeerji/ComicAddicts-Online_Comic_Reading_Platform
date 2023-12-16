namespace SSL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comicsTableCreated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Summary = c.String(),
                        Genre = c.String(),
                        Pages = c.Int(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                        Publisher = c.String(),
                        ReadOnline = c.String(),
                        Download = c.String(),
                        GenreDropDownId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GenreDropdowns", t => t.GenreDropDownId, cascadeDelete: true)
                .Index(t => t.GenreDropDownId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comics", "GenreDropDownId", "dbo.GenreDropdowns");
            DropIndex("dbo.Comics", new[] { "GenreDropDownId" });
            DropTable("dbo.Comics");
        }
    }
}
