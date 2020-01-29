namespace ChemiFriend.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fsbdjkfsdkkhyuuytr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ProductCodeId", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "ProductCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ProductCode", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "ProductCodeId");
        }
    }
}
