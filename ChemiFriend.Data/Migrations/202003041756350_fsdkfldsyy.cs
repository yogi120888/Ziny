namespace ChemiFriend.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fsdkfldsyy : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LicenceImages",
                c => new
                    {
                        ImageId = c.Long(nullable: false, identity: true),
                        ImagePath = c.String(),
                        LicenceId = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: true),
                    })
                .PrimaryKey(t => t.ImageId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LicenceImages");
        }
    }
}
