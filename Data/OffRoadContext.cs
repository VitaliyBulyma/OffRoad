using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OffRoad.Data
{
    public class OffRoadContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public OffRoadContext() : base("name=OffRoadContext")
        {
        }

        public System.Data.Entity.DbSet<OffRoad.Models.Vehicle> Vehicles { get; set; }

        public System.Data.Entity.DbSet<OffRoad.Models.VehicleType> VehicleTypes { get; set; }
     
        public System.Data.Entity.DbSet<OffRoad.Models.Owner> Owners { get; set; }
    }
}
