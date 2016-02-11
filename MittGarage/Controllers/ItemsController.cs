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
    public class ItemsController : Controller
    {
        private ItemContext db = new ItemContext();

        //
        // GET: /Items/

        public ActionResult Index()
        {
            return View(db.Item.ToList());
        }

        //
        // GET: /Items/Details/5

        public ActionResult Details(int id = 0)
        {
            GarageItem garageitem = db.Item.Find(id);
            if (garageitem == null)
            {
                return HttpNotFound();
            }
            return View(garageitem);
        }

        //
        // GET: /Items/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Items/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GarageItem garageitem)
        {
            if (ModelState.IsValid)
            {
                db.Item.Add(garageitem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(garageitem);
        }

        //
        // GET: /Items/Edit/5

        public ActionResult Edit(int id = 0)
        {
            GarageItem garageitem = db.Item.Find(id);
            if (garageitem == null)
            {
                return HttpNotFound();
            }
            return View(garageitem);
        }

        //
        // POST: /Items/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GarageItem garageitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(garageitem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(garageitem);
        }

        public ActionResult Main()
        {
            return View();
        }

        //
        // GET: /Items/Delete/5

        public ActionResult Delete(int id = 0)
        {
            GarageItem garageitem = db.Item.Find(id);
            if (garageitem == null)
            {
                return HttpNotFound();
            }
            return View(garageitem);
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

  
        //
        // POST: /Items/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GarageItem garageitem = db.Item.Find(id);
            db.Item.Remove(garageitem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}