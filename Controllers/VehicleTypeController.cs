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
using System.Diagnostics;


namespace OffRoad.Controllers
{


    public class VehicleTypeController : Controller
    {
        private OffRoadContext db = new OffRoadContext();
        // GET: VehicleType
        public ActionResult Index()
        {
            return View();
        }
                
        public ActionResult List()
        {
          //retrieves data from database of all vehicle types 
            List<VehicleType> myvehicletype = db.VehicleTypes.SqlQuery("Select * from VehicleTypes").ToList();

            return View(myvehicletype);
        }
        public ActionResult Add()
        {


            return View();
        }

        [HttpPost]

        public ActionResult Add(string VehicleTypeName)
        {
            
            //Debug.WriteLine("Value of VehicleTypeName " + VehicleTypeName);
            //adds new vehicle type to the database
            string query = "insert into VehicleTypes (VehicleTypeName) values (@VehicleTypeName)";
            SqlParameter[] sqlparams = new SqlParameter[1];

            sqlparams[0] = new SqlParameter("@VehicleTypeName", VehicleTypeName);
            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }

        public ActionResult Update(int id)
        {
            //retrieves info for a specific vehicle type
            VehicleType selectedvehicletype = db.VehicleTypes.SqlQuery("select * from VehicleTypes where VehicleTypeID = @id", new SqlParameter("@id", id)).FirstOrDefault();

            return View(selectedvehicletype);
        }
        [HttpPost]
        public ActionResult Update(int id, string VehicleTypeName)
        {

            //Debug.WriteLine("I am trying to display VehicleTypeID:" + id+ "and string Name" + VehicleTypeName);

            // updates the record on the submission
            string query = "update VehicleTypes SET  VehicleTypeName=@VehicleTypeName where VehicleTypeID=@id";


            SqlParameter[] sqlparams = new SqlParameter[2];

            sqlparams[0] = new SqlParameter("@VehicleTypeName", VehicleTypeName);
            sqlparams[1] = new SqlParameter("@id", id);


            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }

        public ActionResult ConfirmDelete(int id)
        {
            //retrieves info for a specific vehicle type
            string query = "Select * from VehicleTypes where VehicleTypeID = @id";
            SqlParameter sqlparam = new SqlParameter("@id", id);
            
            VehicleType selectedvehicletype= db.VehicleTypes.SqlQuery(query, sqlparam).FirstOrDefault();
            
            return View(selectedvehicletype);

        }
        public ActionResult Delete(int id)
        {
            //Deletes the record from the database
            string query = "delete from VehicleTypes where VehicleTypeID=@id";
            SqlParameter sqlparam = new SqlParameter("@id", id);
            
            db.Database.ExecuteSqlCommand(query, sqlparam);
            return RedirectToAction("List");
        }

        public ActionResult Show(int id)
        {

            //Displays info for a specific vehicle type
            string query = "Select * from VehicleTypes where VehicleTypeID = @id";
            SqlParameter sqlparam = new SqlParameter("@id", id);
            
            VehicleType selectedvehicletype = db.VehicleTypes.SqlQuery(query, sqlparam).FirstOrDefault();
            
            return View(selectedvehicletype);
        }
      
    }
}