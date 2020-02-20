using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OffRoad.Models.ViewModels
{
    public class UpdateVehicle
    {

        //information about an individual vehicle
        public Vehicle vehicle { get; set; }
        ////information about multiple vehicle types
        public List<VehicleType> vehicletype { get; set; }

    }
}