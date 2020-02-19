using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OffRoad.Models.ViewModels
{
    public class UpdateVehicle
    {
       

        public Vehicle vehicle { get; set; }
        public List<VehicleType> vehicletype { get; set; }

    }
}