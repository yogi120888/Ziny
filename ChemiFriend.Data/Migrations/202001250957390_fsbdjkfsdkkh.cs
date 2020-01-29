namespace ChemiFriend.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fsbdjkfsdkkh : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductCodes",
                c => new
                    {
                        ProductCodeId = c.Int(nullable: false, identity: true),
                        ProductCodes = c.String(maxLength:1000),
                    })
                .PrimaryKey(t => t.ProductCodeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProductCodes");
        }
    }
}
