namespace SSL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesDone : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comics", "GenreDropDownId", "dbo.GenreDropdowns");
            DropIndex("dbo.Comics", new[] { "GenreDropDownId" });
            AlterColumn("dbo.Comics", "GenreDropDownId", c => c.Int());
            CreateIndex("dbo.Comics", "GenreDropDownId");
            AddForeignKey("dbo.Comics", "GenreDropDownId", "dbo.GenreDropdowns", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comics", "GenreDropDownId", "dbo.GenreDropdowns");
            DropIndex("dbo.Comics", new[] { "GenreDropDownId" });
            AlterColumn("dbo.Comics", "GenreDropDownId", c => c.Int(nullable: false));
            CreateIndex("dbo.Comics", "GenreDropDownId");
            AddForeignKey("dbo.Comics", "GenreDropDownId", "dbo.GenreDropdowns", "Id", cascadeDelete: true);
        }
    }
}
