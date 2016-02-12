﻿using System;
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

        public ActionResult Index(string term = null)
        {
            //if (term != null)
            //{
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
            //}
            //return View(db.Item.ToList());
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
        public ActionResult Create(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Item.Add(vehicle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vehicle);
        }

        //
        // GET: /Items/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Vehicle vehicle = db.Item.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        //
        // POST: /Items/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }

        public ActionResult Main()
        {
            return View();
        }

        //
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

        public ActionResult Contact()
        {
            return View();
        }
  
        //
        // POST: /Items/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = db.Item.Find(id);
            db.Item.Remove(vehicle);
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
