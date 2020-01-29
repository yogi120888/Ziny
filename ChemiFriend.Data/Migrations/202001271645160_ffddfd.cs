namespace ChemiFriend.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ffddfd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Schemes", "DealId", "dbo.Deals");
            DropIndex("dbo.Schemes", new[] { "DealId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Schemes", "DealId");
            AddForeignKey("dbo.Schemes", "DealId", "dbo.Deals", "DealId", cascadeDelete: true);
        }
    }
}
