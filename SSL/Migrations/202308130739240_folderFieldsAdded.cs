namespace SSL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class folderFieldsAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comics", "ReadOnline", c => c.String());
            AddColumn("dbo.Comics", "Download", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comics", "Download");
            DropColumn("dbo.Comics", "ReadOnline");
        }
    }
}
