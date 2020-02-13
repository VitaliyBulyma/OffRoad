namespace OffRoad.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Owners",
                c => new
                    {
                        OwnerID = c.Int(nullable: false, identity: true),
                        OwnerFname = c.String(),
                        OwnerLname = c.String(),
                        OwnerNickName = c.String(),
                        OwnerLocation = c.String(),
                    })
                .PrimaryKey(t => t.OwnerID);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        VehicleID = c.Int(nullable: false, identity: true),
                        VehicleMake = c.String(),
                        VehicleModel = c.String(),
                        VehicleYear = c.Int(nullable: false),
                        VehicleColor = c.String(),
                        VehicleEngineSize = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VehicleTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VehicleID)
                .ForeignKey("dbo.Vehicles", t => t.VehicleTypeID)
                .ForeignKey("dbo.VehicleTypes", t => t.VehicleTypeID, cascadeDelete: true)
                .Index(t => t.VehicleTypeID);
            
            CreateTable(
                "dbo.VehicleTypes",
                c => new
                    {
                        VehicleTypeID = c.Int(nullable: false, identity: true),
                        VehicleTypeName = c.String(),
                    })
                .PrimaryKey(t => t.VehicleTypeID);
            
            CreateTable(
                "dbo.VehicleOwners",
                c => new
                    {
                        Vehicle_VehicleID = c.Int(nullable: false),
                        Owner_OwnerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Vehicle_VehicleID, t.Owner_OwnerID })
                .ForeignKey("dbo.Vehicles", t => t.Vehicle_VehicleID, cascadeDelete: true)
                .ForeignKey("dbo.Owners", t => t.Owner_OwnerID, cascadeDelete: true)
                .Index(t => t.Vehicle_VehicleID)
                .Index(t => t.Owner_OwnerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vehicles", "VehicleTypeID", "dbo.VehicleTypes");
            DropForeignKey("dbo.Vehicles", "VehicleTypeID", "dbo.Vehicles");
            DropForeignKey("dbo.VehicleOwners", "Owner_OwnerID", "dbo.Owners");
            DropForeignKey("dbo.VehicleOwners", "Vehicle_VehicleID", "dbo.Vehicles");
            DropIndex("dbo.VehicleOwners", new[] { "Owner_OwnerID" });
            DropIndex("dbo.VehicleOwners", new[] { "Vehicle_VehicleID" });
            DropIndex("dbo.Vehicles", new[] { "VehicleTypeID" });
            DropTable("dbo.VehicleOwners");
            DropTable("dbo.VehicleTypes");
            DropTable("dbo.Vehicles");
            DropTable("dbo.Owners");
        }
    }
}
