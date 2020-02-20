using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OffRoad.Models.ViewModels;

namespace OffRoad.Models.ViewModels
{
    public class ShowOwner
    {

        //information about an individual owner
        public virtual Owner owner { get; set; }
        //information about  vehicles they own
        public List<Vehicle> vehicles { get; set; }
        //information about all  vehicles to populate drop-down list
        public List<Vehicle> all_vehicles { get; set; }

    }
}