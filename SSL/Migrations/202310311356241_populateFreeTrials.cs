namespace SSL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class populateFreeTrials : DbMigration
    {
        public override void Up()
        {
            Sql("Insert into FreeTrials(Name,Trials,Downloads)values('Free',5,0)");
            Sql("Insert into FreeTrials(Name,Trials,Downloads)values('Silver',99999,10)");
            Sql("Insert into FreeTrials(Name,Trials,Downloads)values('Gold',99999,99999)");

        }

        public override void Down()
        {
        }
    }
}
