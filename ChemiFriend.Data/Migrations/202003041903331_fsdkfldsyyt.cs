namespace ChemiFriend.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fsdkfldsyyt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LicenceImages", "RegistrationId", c => c.Long(nullable: false));
            DropColumn("dbo.LicenceImages", "LicenceId");
            DropColumn("dbo.Registrations", "LicenceImageId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Registrations", "LicenceImageId", c => c.Long(nullable: false));
            AddColumn("dbo.LicenceImages", "LicenceId", c => c.Long(nullable: false));
            DropColumn("dbo.LicenceImages", "RegistrationId");
        }
    }
}
