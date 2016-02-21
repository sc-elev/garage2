using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using MittGarage.Models;
using MittGarage.DataAccessLayer;
using System.Configuration;


namespace MittGarage.Controllers
{
    
    public class BaseController : Controller
    {
        private ItemContext db = new ItemContext();

        public Garage garage
        {
            private set { garage = value; }
            get
            {
                if (TempData["garage"] == null)
                {
                    string GarageId = ConfigurationManager.AppSettings["GarageId"].ToString();
                    uint capacity = 
                        (uint)int.Parse(ConfigurationManager.AppSettings["GarageCapacity"].ToString());
                    TempData["garage"] = new  Garage(GarageId, capacity);
                }
                return (Garage)TempData["garage"];
            }
        }
    }
}