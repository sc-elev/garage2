namespace MittGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GarageItems");
        }
    }
}
