using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelManagementSystem.Models;

namespace HotelManagementSystem.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdultsController : Controller
    {
        private HotelManagementDB_DataEntities db = new HotelManagementDB_DataEntities();

        // GET: Adults
        public ActionResult Index()
        {
            return View(db.Adults.ToList());
        }

        // GET: Adults/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adult adult = db.Adults.Find(id);
            if (adult == null)
            {
                return HttpNotFound();
            }
            return View(adult);
        }

        // GET: Adults/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Adults/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdultId,NumberOfAdults")] Adult adult)
        {
            if (ModelState.IsValid)
            {
                db.Adults.Add(adult);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adult);
        }

        // GET: Adults/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adult adult = db.Adults.Find(id);
            if (adult == null)
            {
                return HttpNotFound();
            }
            return View(adult);
        }

        // POST: Adults/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdultId,NumberOfAdults")] Adult adult)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adult).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(adult);
        }

        // GET: Adults/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adult adult = db.Adults.Find(id);
            if (adult == null)
            {
                return HttpNotFound();
            }
            return View(adult);
        }

        // POST: Adults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Adult adult = db.Adults.Find(id);
            db.Adults.Remove(adult);
            db.SaveChanges();
            return RedirectToAction("Index");
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
