using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MittGarage.Models;
using MittGarage.DataAccessLayer;

namespace MittGarage.Controllers
{
    public class ItemsController : BaseController
    {
        private ItemContext db = new ItemContext();

        #region Items
        // GET: /Items/

        public ActionResult Index(string term = null)
        {
            if (term != null)
            {
                var model = db.Item.OrderBy(r => r.RegNr).Where
				(r => 
                    r.RegNr.Equals(term) || 
					r.Owner.Equals(term)
                    )

                    .ToList();

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_centrallagret", model);
                }

                return View(model);
            }
            return View(db.Item.ToList());
        }

        #endregion Items

        #region Items/Details/5

        public ActionResult List(SearchCtx ctx)
        {
            return View(garage.Search(ctx));
        }

        public ActionResult List(List<Vehicle> vehicles)
        {
            return View(vehicles);
        }
        

        //
        // GET: /Items/Details/5

        public ActionResult Details(int id = 0)
        {
            Vehicle vehicle = db.Item.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
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
                db.Item.Add(vehicle);
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
            Vehicle vehicle = db.Item.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
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

        public ActionResult Delete(int id = 0)
        {
            Vehicle vehicle = db.Item.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        public ActionResult CheckIn()
        {
            return View();
        }

        public ActionResult Checkout()
        {
            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }

        [HttpPost, ActionName("Admin")]
//        [ValidateAntiForgeryToken]
        public ActionResult Admin(SearchCtx ctx)
        {
            Garage garage = new Garage("default", 50);
            List<Vehicle> found = garage.Search(ctx).ToList();
            if (found.Count == 0) return HttpNotFound();
            return View(found);
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
            Vehicle vehicle = db.Item.Find(id);
            db.Item.Remove(vehicle);
            db.SaveChanges();
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
