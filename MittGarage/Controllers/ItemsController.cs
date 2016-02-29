using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MittGarage.Models;
using MittGarage.DataAccessLayer;
using PagedList;

namespace MittGarage.Controllers
{
    public class ItemsController : BaseController
    {
        private GarageDbContext db = new GarageDbContext();

        #region Items
        // GET: /Items/

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Index(string term = null)
        {
            if (term != null)
            {
                var model = db.Vehicles.OrderBy(r => r.RegNr)
                    .Where(r => r.RegNr == term || r.OwnerName == term)
                    .ToList();
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_centrallagret", model);
                }

                return View(model);
            }
            return View(db.Vehicles.ToList());
        }

        public ActionResult CheckOut()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(string term)
        {
            if (term == null) return RedirectToAction("NotFound");

            var model = garage.Search(term);
            return View(model);
        }

        #endregion Items

        #region Items/Details/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(SearchCtx ctx)
        {
            TempData["vehicles"] = garage.Search(ctx).ToList<Vehicle>();
            return List();
        }


        public ActionResult List()
        {
            List<Vehicle> vehicles = (List<Vehicle>)TempData["vehicles"];
            if (vehicles.Count == 0) return RedirectToAction("NotFound");
            return View(vehicles);
        }

        // GET: /Items/Details/5

        public ActionResult Details(int id = 0)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return RedirectToAction("NotFound");
            }
            return View(vehicle);
        }
        #endregion Items/Details/5

        #region --Items/Create--

        #region GET
        // GET: /Items/Create

        public ActionResult Create()
        {
            return View();
        }
        #endregion GET

        #region POST
        // POST: /Items/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Vehicles.Add(vehicle);
                db.SaveChanges();
                return RedirectToAction("Main");
            }

            return View(vehicle);
        }
        #endregion POST

        #endregion --Items/Create--

        #region --Items/Edit/5--

        #region GET
        // GET: /Items/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null) return RedirectToAction("NotFound");
            TempData["vehicles"] = new List<Vehicle> { vehicle };
            return RedirectToAction("List");
        }
        #endregion GET

        #region POST
        // POST: /Items/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vehicle garageitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(garageitem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Main");
            }
            return View(garageitem);
        }

        public ActionResult Main()
        {
            return View();
        }
        #endregion POST

        #endregion --Items/Edit/5--

        #region --Items/Delete/5--

        #region GET
        // GET: /Items/Delete/5

        public ActionResult Delete(int id)
        {
            Vehicle vehicle = garage.VehicleById(id);
            if (vehicle == null)
            {
                return RedirectToAction("NotFound");
            }
            return View(vehicle);
        }

        public ActionResult CheckIn()
        {
            return View();
        }

        public ActionResult CheckInOK()
        {
            var vehicle = (Vehicle)TempData["vehicle"];
            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckIn(VehicleCtx ctx)
        {
            var car = new Vehicle(ctx.Owner);
            car.RegNr = ctx.RegNr;
            car.VTID = 1;
            garage.Add(car);
            TempData["vehicle"] = car;
            return RedirectToAction("CheckinOK");
        }

        private ActionResult ShowAdmin(SearchCtx ctx, int page = 1)
        {
            if (ctx == null) ctx = (SearchCtx)TempData["SearchCtx"];
            if (ctx == null) ctx = new SearchCtx();
            ctx.TypesById = garage.TypesById();
            if (ctx.Results.Count == 0)
            {
                ctx.Results = garage.Search(ctx).ToList();
            }
            ctx.PagedResults = ctx.Results.ToPagedList(page, 10);

            TempData["vehicles"] = ctx.Results;
            TempData["SearchCtx"] = ctx;
            return View(ctx);
        }

        public ActionResult Admin(int page = 1)
        {
            return ShowAdmin(null, page);
        }

        [HttpPost, ActionName("Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Admin(SearchCtx ctx, int page = 1)
        {
            return ShowAdmin(ctx, page);
        }

        public JsonResult GetVehicles()
        {
            var vehicles = garage.ListAll();
            return Json(vehicles, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTypes()
        {
            var types = garage.TypesById().Values;
            return Json(types, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Sort(string sortOrder)
        {
            SearchCtx  ctx = (SearchCtx)TempData["SearchCtx"];
            if (ctx == null)
                ctx = new SearchCtx();
            ctx.Sort(sortOrder);
            TempData["SearchCtx"] = ctx;
            TempData["vehicles"] = ctx.Results;
            return RedirectToAction("Admin", new { page = 1 });
        }

        public ActionResult Contact()
        {
            return View();
        }
        #endregion GET

        #region POST
        // POST: /Items/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Where(v => v.Id == id).First();
            db.Vehicles.Remove(vehicle);
            if (TempData.ContainsKey("SearchCtx"))
            {
                ((SearchCtx)TempData["SearchCtx"]).Results =
                    new List<Vehicle>();
            }
            return RedirectToAction("Main");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        #endregion POST

        #endregion --Items/Delete/5--
    }
}
