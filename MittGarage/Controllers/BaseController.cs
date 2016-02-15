using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MittGarage.Models;
using MittGarage.DataAccessLayer;


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
                return new Garage("default", 50);
            }
        }
    }
}