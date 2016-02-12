namespace MittGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Owner = c.String(),
                        Type = c.Int(nullable: false),
                        checkInDate = c.DateTime(nullable: false),
                        Color = c.Int(nullable: false),
                        Wheels = c.Int(nullable: false),
                        RegNr = c.String(),
                        Brand = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.GarageItems");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GarageItems",
                c => new
                    {
                        ItemID = c.Int(nullable: false, identity: true),
                        Vehicle = c.Int(nullable: false),
                        regNR = c.String(),
                        Owner = c.String(),
                        Inchecked = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ItemID);
            
            DropTable("dbo.Vehicles");
        }
    }
}
