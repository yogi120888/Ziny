namespace ChemiFriend.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fbhsjfsiyy : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "PaymentStatus", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "PaymentStatus", c => c.Boolean(nullable: false));
        }
    }
}
