namespace SSL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class genresTableCreated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GenreDropdowns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GenreDropdowns");
        }
    }
}
