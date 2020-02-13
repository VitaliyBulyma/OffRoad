using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace OffRoad.Models
{
    public class Owner
    {
        [Key]
        public int OwnerID { get; set; }
        public string OwnerFname { get; set; }
        public string OwnerLname { get; set; }
        public string OwnerNickName { get; set; }
        public string OwnerLocation { get; set; }

        //Represents the "Many-to- Many" relationship to Vehicles
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}