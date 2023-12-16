namespace SSL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class populateGenres : DbMigration
    {
        public override void Up()
        {
            Sql("Insert into GenreDropdowns(Name) VALUES ('Action')");
            Sql("Insert into GenreDropdowns(Name) VALUES ('Superhero')");
            Sql("Insert into GenreDropdowns(Name) VALUES ('Horror')");
            Sql("Insert into GenreDropdowns(Name) VALUES ('Adventure')");
            Sql("Insert into GenreDropdowns(Name) VALUES ('Fantasy')");
        }
        
        public override void Down()
        {
        }
    }
}
