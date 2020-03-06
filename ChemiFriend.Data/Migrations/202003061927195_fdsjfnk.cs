namespace ChemiFriend.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fdsjfnk : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LicenceImages",
                c => new
                    {
                        ImageId = c.Long(nullable: false, identity: true),
                        ImagePath = c.String(),
                        RegistrationId = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ImageId);
            
            AlterColumn("dbo.Deals", "ApplicableTaxType", c => c.Int());
            DropColumn("dbo.Registrations", "LicenceImage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Registrations", "LicenceImage", c => c.String());
            AlterColumn("dbo.Deals", "ApplicableTaxType", c => c.Int(nullable: false));
            DropTable("dbo.LicenceImages");
        }
    }
}
