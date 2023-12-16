namespace SSL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfilePictureAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ProfilePicture", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ProfilePicture");
        }
    }
}
