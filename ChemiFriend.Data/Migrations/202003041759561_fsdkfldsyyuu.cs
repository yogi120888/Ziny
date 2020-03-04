namespace ChemiFriend.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fsdkfldsyyuu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Registrations", "LicenceImageId", c => c.Long(nullable: false));
            DropColumn("dbo.Registrations", "LicenceImage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Registrations", "LicenceImage", c => c.String());
            DropColumn("dbo.Registrations", "LicenceImageId");
        }
    }
}
