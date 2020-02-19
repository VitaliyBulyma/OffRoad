using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OffRoad.Models.ViewModels
{
    public class ShowVehicle
    {
        //information about an individual vehicle
        public virtual Vehicle vehicle { get; set; }

        //information about multiple owners
        public List<Owner> owners { get; set; }
    }
}