namespace ChemiFriend.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fsbdjkfsdkkhyuu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductCodes", "Status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductCodes", "Status");
        }
    }
}
