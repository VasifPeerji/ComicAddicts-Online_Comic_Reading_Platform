namespace SSL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class whatChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ratings", "Stars", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ratings", "Stars", c => c.Int(nullable: false));
        }
    }
}
