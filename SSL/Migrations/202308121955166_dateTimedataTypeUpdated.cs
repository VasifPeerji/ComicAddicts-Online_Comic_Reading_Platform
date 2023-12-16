namespace SSL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dateTimedataTypeUpdated : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comics", "DateAdded", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comics", "DateAdded", c => c.Int(nullable: false));
        }
    }
}
