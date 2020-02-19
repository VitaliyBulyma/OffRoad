using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OffRoad.Models
{
    public class Vehicle
    {
        [Key]
        public int VehicleID { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public int VehicleYear { get; set; }
        public string VehicleColor { get; set; }
        public decimal VehicleEngineSize { get; set; }

        public int VehicleTypeID { get; set; }
        [ForeignKey("VehicleTypeID")]
        public virtual VehicleType VehicleType { get; set; }

        public ICollection<Owner> Owners { get; set; }
    }
}