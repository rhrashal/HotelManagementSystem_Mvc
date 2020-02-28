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
    [Authorize(Roles = "Admin")]
    public class RoomQuantitiesController : Controller
    {
        private HotelManagementDB_DataEntities db = new HotelManagementDB_DataEntities();

        // GET: RoomQuantities
        public ActionResult Index()
        {
            return View(db.RoomQuantities.ToList());
        }

        // GET: RoomQuantities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomQuantity roomQuantity = db.RoomQuantities.Find(id);
            if (roomQuantity == null)
            {
                return HttpNotFound();
            }
            return View(roomQuantity);
        }

        // GET: RoomQuantities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoomQuantities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomQuantityId,NumberOfRoom")] RoomQuantity roomQuantity)
        {
            if (ModelState.IsValid)
            {
                db.RoomQuantities.Add(roomQuantity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(roomQuantity);
        }

        // GET: RoomQuantities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomQuantity roomQuantity = db.RoomQuantities.Find(id);
            if (roomQuantity == null)
            {
                return HttpNotFound();
            }
            return View(roomQuantity);
        }

        // POST: RoomQuantities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomQuantityId,NumberOfRoom")] RoomQuantity roomQuantity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomQuantity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roomQuantity);
        }

        // GET: RoomQuantities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomQuantity roomQuantity = db.RoomQuantities.Find(id);
            if (roomQuantity == null)
            {
                return HttpNotFound();
            }
            return View(roomQuantity);
        }

        // POST: RoomQuantities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoomQuantity roomQuantity = db.RoomQuantities.Find(id);
            db.RoomQuantities.Remove(roomQuantity);
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
