namespace ChemiFriend.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dsnfndslfppp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Deals", "ApplicableTaxType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Deals", "ApplicableTaxType");
        }
    }
}
