using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OffRoad.Models.ViewModels;

namespace OffRoad.Models.ViewModels
{
    public class ShowOwner
    {

       
        public virtual Owner owner { get; set; }
        
        public List<Vehicle> vehicles { get; set; }

        
        public List<Vehicle> all_vehicles { get; set; }

    }
}