﻿using System;
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
using OffRoad.Models.ViewModels;

namespace OffRoad.Controllers
{
    public class VehicleController : Controller
    {
        
        private OffRoadContext db = new OffRoadContext();

        // GET: Vehicle
        public ActionResult List(string vehiclesearchkey)
        {
            
            string query = "Select * from Vehicles ";

            //if search is entered string query will be modified before passing it into list query
            if (vehiclesearchkey != "")
            {
                query= query+ "where VehicleMake like '%"+vehiclesearchkey+ "%' or VehicleModel like '%" + vehiclesearchkey + "%'  ";
            }
            List<Vehicle> vehicles = db.Vehicles.SqlQuery(query).ToList();
            return View(vehicles);

        }

        
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Vehicle vehicle = db.Vehicles.SqlQuery("select * from Vehicles where VehicleID=@VehicleID", new SqlParameter("@VehicleID", id)).FirstOrDefault();
            if (vehicle == null)
            {
                return HttpNotFound();
            }


            //need information about the list of owners associated with that vehicle
            string query = "select * from Owners inner join VehicleOwners on Owners.OwnerID = VehicleOwners.Owner_OwnerID where Vehicle_VehicleID = @id";
            SqlParameter param = new SqlParameter("@id", id);
            List<Owner> VehicleOwners = db.Owners.SqlQuery(query, param).ToList();


            ShowVehicle viewmodel = new ShowVehicle();
            viewmodel.vehicle = vehicle;
            viewmodel.owners = VehicleOwners;


            return View(viewmodel);
        }

       
        [HttpPost]
        public ActionResult Add(string VehicleMake, string VehicleModel, int VehicleYear, string VehicleColor,decimal VehicleEngineSize, int VehicleTypeID)
        {
           
            string query = "insert into Vehicles (VehicleMake,VehicleModel, VehicleYear, VehicleColor, VehicleEngineSize,  VehicleTypeID) values (@VehicleMake,@VehicleModel, @VehicleYear, @VehicleColor, @VehicleEngineSize,  @VehicleTypeID)";
            SqlParameter[] sqlparams = new SqlParameter[6]; 
            //each piece of information is a key and value pair
            sqlparams[0] = new SqlParameter("@VehicleMake", VehicleMake);
            sqlparams[1] = new SqlParameter("@VehicleModel", VehicleModel);
            sqlparams[2] = new SqlParameter("@VehicleYear", VehicleYear);
            sqlparams[3] = new SqlParameter("@VehicleColor", VehicleColor);
            sqlparams[4] = new SqlParameter("@VehicleEngineSize", VehicleEngineSize);
            sqlparams[5] = new SqlParameter("@VehicleTypeID", VehicleTypeID);
                      
            db.Database.ExecuteSqlCommand(query, sqlparams);
                       
            return RedirectToAction("List");
        }


        public ActionResult Add ()
        {
           

            List<VehicleType> vehicletypes = db.VehicleTypes.SqlQuery("select * from VehicleTypes").ToList();

            return View(vehicletypes);
        }

        public ActionResult Update(int id)
        {
            
            Vehicle selectedvehicle = db.Vehicles.SqlQuery("select * from Vehicles where VehicleID = @id", new SqlParameter("@id", id)).FirstOrDefault();

            List<VehicleType> vehicletypes = db.VehicleTypes.SqlQuery("select * from VehicleTypes").ToList();


            UpdateVehicle viewmodel = new UpdateVehicle();
            viewmodel.vehicle = selectedvehicle;
            viewmodel.vehicletype = vehicletypes;
            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult Update(int id, string VehicleMake, string VehicleModel, int VehicleYear, string VehicleColor, decimal VehicleEngineSize, int VehicleTypeID)
        {
            Debug.WriteLine("I am trying to display VehicleID:" + id + 
                "VehicleMake: "  + VehicleMake+
                "VehicleModel: " + VehicleModel +
                "VehicleYear: " + VehicleYear +
                "VehicleColor: " + VehicleColor +
                "VehicleEngineSize: " + VehicleEngineSize +
                "VehicleTypeID: " + VehicleTypeID 



                );
            string queryUpdate = "update Vehicles SET VehicleMake=@VehicleMake , VehicleTypeID=@VehicleTypeID, VehicleModel=@VehicleModel , VehicleYear=@VehicleYear, VehicleColor=@VehicleColor, VehicleEngineSize=@VehicleEngineSize where VehicleID=@id";

            SqlParameter[] sqlparams = new SqlParameter[7];

            sqlparams[0] = new SqlParameter("@VehicleMake", VehicleMake);
            sqlparams[1] = new SqlParameter("@VehicleModel", VehicleModel);
            sqlparams[2] = new SqlParameter("@VehicleYear", VehicleYear);
            sqlparams[3] = new SqlParameter("@id",id);
            sqlparams[4] = new SqlParameter("@Vehiclecolor", VehicleColor);
            sqlparams[5] = new SqlParameter("@VehicleEngineSize", VehicleEngineSize);
            sqlparams[6] = new SqlParameter("@VehicleTypeID", VehicleTypeID);

            db.Database.ExecuteSqlCommand(queryUpdate, sqlparams);

            return RedirectToAction("List");
        }


        public ActionResult ConfirmDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
         
            Vehicle vehicle = db.Vehicles.SqlQuery("select * from Vehicles where VehicleID=@VehicleID", new SqlParameter("@VehicleID", id)).FirstOrDefault();
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }
        public ActionResult Delete(int id)
        {

            string query = "delete from Vehicles where VehicleID=@id";
            SqlParameter sqlparam = new SqlParameter("@id", id);


            db.Database.ExecuteSqlCommand(query, sqlparam);
            return RedirectToAction("List");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
