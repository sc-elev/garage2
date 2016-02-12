using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MittGarage.Controllers
{
    public class HomeController : Controller
    {
        #region Index
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }
        #endregion Index

        #region About
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }
        #endregion About

        #region Contact
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        #endregion About
    }
}
