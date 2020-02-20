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
using System.IO;

namespace OffRoad.Controllers
{
    public class VehicleController : Controller
    {
        
        private OffRoadContext db = new OffRoadContext();

        // GET: Vehicle
        public ActionResult List(string vehiclesearchkey)
        {
            //Debug.WriteLine("Search is:  " + vehiclesearchkey);

            //retrieves data from database to populate list of all vehicles
            string query = "Select * from Vehicles ";

            //if search is entered string query will be modified before passing it into list query
            if (vehiclesearchkey != "")
            {
                query= query+ "where VehicleMake like '%"+vehiclesearchkey+ "%' or VehicleModel like '%" + vehiclesearchkey + "%'  ";
            }
            List<Vehicle> vehicles = db.Vehicles.SqlQuery(query).ToList();
            
            // I want to send a message if search returned nothing
            
            //Debug.WriteLine("vehicle count is " + vehicles.Count);
            //if (vehicles.Count == 0)
            //{
            //    string emptyset = "Your search returned no results";
            //}

            return View(vehicles);

        }

        
        public ActionResult Show(int? id)
        {
            // if no id passed, the error will display
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Vehicle vehicle = db.Vehicles.SqlQuery("select * from Vehicles where VehicleID=@VehicleID", new SqlParameter("@VehicleID", id)).FirstOrDefault();
            
            //if there are no vehicles in database
            if (vehicle == null)
            {
                return HttpNotFound();
            }


            //retrieves the list of owners associated with that vehicle
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
           //adds new vehicle to the databse
            string query = "insert into Vehicles (VehicleMake,VehicleModel, VehicleYear, VehicleColor, VehicleEngineSize,  VehicleTypeID) values (@VehicleMake,@VehicleModel, @VehicleYear, @VehicleColor, @VehicleEngineSize,  @VehicleTypeID)";
            SqlParameter[] sqlparams = new SqlParameter[6]; 
            
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
           
            //populates a drop-down list of all vehicle types available 
            List<VehicleType> vehicletypes = db.VehicleTypes.SqlQuery("select * from VehicleTypes").ToList();

            return View(vehicletypes);
        }

        public ActionResult Update(int id)
        {
            //retrieves and populates the data for specific vehicle
            Vehicle selectedvehicle = db.Vehicles.SqlQuery("select * from Vehicles where VehicleID = @id", new SqlParameter("@id", id)).FirstOrDefault();

            List<VehicleType> vehicletypes = db.VehicleTypes.SqlQuery("select * from VehicleTypes").ToList();
            
            UpdateVehicle viewmodel = new UpdateVehicle();
            viewmodel.vehicle = selectedvehicle;
            viewmodel.vehicletype = vehicletypes;
            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult Update(int id, string VehicleMake, string VehicleModel, int VehicleYear, string VehicleColor, decimal VehicleEngineSize, int VehicleTypeID, HttpPostedFileBase VehicleFoto)
        {
            //Debug.WriteLine("I am trying to display VehicleID:" + id + 
            //    "VehicleMake: "  + VehicleMake+
            //    "VehicleModel: " + VehicleModel +
            //  "VehicleYear: " + VehicleYear +
            //    "VehicleColor: " + VehicleColor +
            //    "VehicleEngineSize: " + VehicleEngineSize +
            //   "VehicleTypeID: " + VehicleTypeID 
            //    );

            int haspic = 0;
            string vehiclepicextension = "";

            
            if (VehicleFoto != null)
            {
                //Debug.WriteLine("Something identified...");
                //checking to see if the file size is greater than 0 (bytes)
                if (VehicleFoto.ContentLength > 0)
                {
                    //Debug.WriteLine("Successfully Identified Image");
                    //file extensioncheck taken from https://www.c-sharpcorner.com/article/file-upload-extension-validation-in-asp-net-mvc-and-javascript/
                    var valtypes = new[] { "jpeg", "jpg", "png", "gif", "JPG","MP4" };
                    var extension = Path.GetExtension(VehicleFoto.FileName).Substring(1);

                    if (valtypes.Contains(extension))
                    {
                        try
                        {
                            //file name is the id of the image
                            string fn = id + "." + extension;

                            //get a direct file path to ~/Content/Vehicles/{id}.{extension}
                            string path = Path.Combine(Server.MapPath("~/Content/Vehicles/"), fn);

                            //save the file
                            VehicleFoto.SaveAs(path);
                            //if these are all successful then we can set these fields
                            haspic = 1;
                            vehiclepicextension = extension;

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Vehicle Image was not saved successfully.");
                            Debug.WriteLine("Exception:" + ex);
                        }



                    }
                }
            }

            
            //updates vehicle information
        string queryUpdate = "update Vehicles SET VehicleMake=@VehicleMake , VehicleTypeID=@VehicleTypeID, VehicleModel=@VehicleModel , VehicleYear=@VehicleYear, VehicleColor=@VehicleColor, VehicleEngineSize=@VehicleEngineSize,HasPic=@haspic, PicExtension=@vehiclepicextension where VehicleID=@id";

            SqlParameter[] sqlparams = new SqlParameter[9];

            sqlparams[0] = new SqlParameter("@VehicleMake", VehicleMake);
            sqlparams[1] = new SqlParameter("@VehicleModel", VehicleModel);
            sqlparams[2] = new SqlParameter("@VehicleYear", VehicleYear);
            sqlparams[3] = new SqlParameter("@id",id);
            sqlparams[4] = new SqlParameter("@Vehiclecolor", VehicleColor);
            sqlparams[5] = new SqlParameter("@VehicleEngineSize", VehicleEngineSize);
            sqlparams[6] = new SqlParameter("@VehicleTypeID", VehicleTypeID);
            sqlparams[7] = new SqlParameter("@HasPic", haspic);
            sqlparams[8] = new SqlParameter("@vehiclepicextension", vehiclepicextension);

        db.Database.ExecuteSqlCommand(queryUpdate, sqlparams);

            return RedirectToAction("List");
        }


        public ActionResult ConfirmDelete(int? id)
        {
            //shows error if no id passed
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
            //deletes vehicle from the database
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
