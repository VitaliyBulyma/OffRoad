using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace OffRoad.Models
{
    public class VehicleType
    {
        [Key]
        public int VehicleTypeID { get; set; }
        public string VehicleTypeName { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }

    }
}