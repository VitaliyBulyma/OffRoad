using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OffRoad.Data;
using OffRoad.Models;
using OffRoad.Models.ViewModels;
using System.Diagnostics;


namespace OffRoad.Controllers
{


    public class OwnerController : Controller
    {
        private OffRoadContext db = new OffRoadContext();
        // GET: Owner
        public ActionResult Index()
        {
            return View();
        }
               
        public ActionResult List(string ownersearchkey)
        {
            string query = "Select * from Owners ";

            //if search is entered string query will be modified before passing it into list query
            if (ownersearchkey != "")
            {
                query = query + "where OwnerFname like '%" + ownersearchkey + "%' or OwnerLname like '%" + ownersearchkey + "%' or OwnerNickName like '%" + ownersearchkey + "%'  ";
            }


            List<Owner> myowner = db.Owners.SqlQuery(query).ToList();

            return View(myowner);
        }
        

        public ActionResult AttachVehicle(int id, int VehicleID)
        {
            Debug.WriteLine("owner id is" + id + " and vehicleid is " + VehicleID);

            string check_query = "select * from Vehicles inner join VehicleOwners on VehicleOwners.Vehicle_VehicleID = Vehicles.VehicleID where Vehicle_VehicleID=@VehicleID and Owner_OwnerID=@id";
            SqlParameter[] check_params = new SqlParameter[2];
            check_params[0] = new SqlParameter("@id", id);
            check_params[1] = new SqlParameter("@VehicleID", VehicleID);
            List<Vehicle> vehicles = db.Vehicles.SqlQuery(check_query, check_params).ToList();
            
            if (vehicles.Count <= 0)
            {


                
                string query = "insert into VehicleOwners (Vehicle_VehicleID, Owner_OwnerID) values (@VehicleID, @id)";
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@id", id);
                sqlparams[1] = new SqlParameter("@VehicleID", VehicleID);


                db.Database.ExecuteSqlCommand(query, sqlparams);
            }

            return RedirectToAction("Show/" + id);

        }


     
        [HttpGet]
        public ActionResult DetachVehicle(int id, int VehicleID)
        {
           
            Debug.WriteLine("owner id is" + id + " and vehicleid is " + VehicleID);

            string query = "delete from VehicleOwners where Vehicle_VehicleID=@VehicleID and Owner_OwnerID=@id";
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@VehicleID", VehicleID);
            sqlparams[1] = new SqlParameter("@id", id);

            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("Show/" + id);
        }

       
        public ActionResult Add()
        {

            return View();
        }

        [HttpPost]

        public ActionResult Add(string OwnerFname, string OwnerLname, string OwnerNickName, string OwnerLocation)
        {
            // Debug.WriteLine("Value of OwnerFname " + OwnerFname +
            //                "Value of OwnerLname " + OwnerLname +
            //                "Value of OwnerNickName " + OwnerNickName +
            //               "Value of OwnerLocation " + OwnerLocation);

            string query = "insert into Owners (OwnerFname, OwnerLname, OwnerNickName, OwnerLocation) values (@OwnerFname, @OwnerLname, @OwnerNickName, @OwnerLocation)";
            SqlParameter[] sqlparams = new SqlParameter[4];

            sqlparams[0] = new SqlParameter("@OwnerFname", OwnerFname);
            sqlparams[1] = new SqlParameter("@OwnerLname", OwnerLname);
            sqlparams[2] = new SqlParameter("@OwnerNickName", OwnerNickName);
            sqlparams[3] = new SqlParameter("@OwnerLocation", OwnerLocation);
            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }

        public ActionResult Update(int id)
        {

            Owner selectedowner = db.Owners.SqlQuery("select * from Owners where OwnerID = @id", new SqlParameter("@id", id)).FirstOrDefault();

            return View(selectedowner);
        }
        [HttpPost]
        public ActionResult Update(int id, string OwnerFname, string OwnerLname, string OwnerNickName, string OwnerLocation)
        {


            Debug.WriteLine("Value of OwnerFname " + OwnerFname +
                            "Value of OwnerLname " + OwnerLname +
                            "Value of OwnerNickName " + OwnerNickName +
                            "Value of OwnerLocation " + OwnerLocation);

            string query = "update Owners SET  OwnerFname=@OwnerFname, OwnerLname=@OwnerLname, OwnerNickName=@OwnerNickName, OwnerLocation=@OwnerLocation where OwnerID=@id";


            SqlParameter[] sqlparams = new SqlParameter[5];

            sqlparams[0] = new SqlParameter("@OwnerFname", OwnerFname);
            sqlparams[1] = new SqlParameter("@OwnerLname", OwnerLname);
            sqlparams[2] = new SqlParameter("@OwnerNickName", OwnerNickName);
            sqlparams[3] = new SqlParameter("@OwnerLocation", OwnerLocation);
            sqlparams[4] = new SqlParameter("@id", id);


            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }

        public ActionResult ConfirmDelete(int id)
        {
            string query = "Select * from Owners where OwnerID = @id";
            SqlParameter sqlparam = new SqlParameter("@id", id);


            Owner selectedowner = db.Owners.SqlQuery(query, sqlparam).FirstOrDefault();


            return View(selectedowner);

        }
        public ActionResult Delete(int id)
        {

            string query = "delete from Owners where OwnerID=@id";
            SqlParameter sqlparam = new SqlParameter("@id", id);


            db.Database.ExecuteSqlCommand(query, sqlparam);
            return RedirectToAction("List");
        }

        public ActionResult Show(int id)
        {


           // string query = "Select * from Owners where OwnerID = @id";
           // SqlParameter sqlparam = new SqlParameter("@id", id);


            //Owner selectedowner = db.Owners.SqlQuery(query, sqlparam).FirstOrDefault();


            //return View(selectedowner);
           
            
            string main_query = "select * from Owners where OwnerID = @id";
            var pk_parameter = new SqlParameter("@id", id);
            Owner Owner = db.Owners.SqlQuery(main_query, pk_parameter).FirstOrDefault();

           
            string aside_query = "select * from Vehicles inner join VehicleOwners on Vehicles.VehicleID = VehicleOwners.Vehicle_VehicleID where VehicleOwners.Owner_OwnerID=@id";
            var fk_parameter = new SqlParameter("@id", id);
            List<Vehicle> OwnedVehicles = db.Vehicles.SqlQuery(aside_query, fk_parameter).ToList();

            string all_vehicles_query = "select * from Vehicles";
            List<Vehicle> AllVehicles = db.Vehicles.SqlQuery(all_vehicles_query).ToList();
                       
            ShowOwner viewmodel = new ShowOwner();
            viewmodel.owner = Owner;
            viewmodel.vehicles = OwnedVehicles;
            viewmodel.all_vehicles = AllVehicles;

            return View(viewmodel);

            
        }
   
    }
}