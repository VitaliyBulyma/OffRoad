namespace OffRoad.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VehicleType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vehicles", "VehicleTypeID", "dbo.Vehicles");
        }
        
        public override void Down()
        {
            AddForeignKey("dbo.Vehicles", "VehicleTypeID", "dbo.Vehicles", "VehicleID");
        }
    }
}
