namespace OffRoad.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class picextension : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "HasPic", c => c.Int(nullable: false));
            AddColumn("dbo.Vehicles", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vehicles", "PicExtension");
            DropColumn("dbo.Vehicles", "HasPic");
        }
    }
}
