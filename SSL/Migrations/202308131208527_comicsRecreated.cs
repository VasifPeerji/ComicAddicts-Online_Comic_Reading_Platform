namespace SSL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comicsRecreated : DbMigration
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
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Comics");
        }
    }
}
