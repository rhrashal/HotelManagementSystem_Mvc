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
    public class BookingsController : Controller
    {
        private HotelManagementDB_DataEntities db = new HotelManagementDB_DataEntities();

        // GET: Bookings
        public ActionResult Index()
        {
            var bookings = db.Bookings.Include(b => b.Adult).Include(b => b.BeddingType).Include(b => b.Child).Include(b => b.RoomQuantity).Include(b => b.RoomType);
            return View(bookings.ToList());
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }
        [AllowAnonymous]
        // GET: Bookings/Create
        public ActionResult Create()
        {
            ViewBag.AdultId = new SelectList(db.Adults, "AdultId", "AdultId");
            ViewBag.BeddingTypeId = new SelectList(db.BeddingTypes, "BeddingTypeId", "BeddingTypeName");
            ViewBag.ChildId = new SelectList(db.Children, "ChildId", "ChildId");
            ViewBag.RoomQuantityId = new SelectList(db.RoomQuantities, "RoomQuantityId", "RoomQuantityId");
            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "RoomTypeId", "RoomTypeName");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingId,FirstName,LastName,Address,Email,Phone,NID_No,ArrivalTime,DepartureDate,AdultId,ChildId,RoomTypeId,BeddingTypeId,RoomQuantityId,Notes")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.AdultId = new SelectList(db.Adults, "AdultId", "AdultId", booking.AdultId);
            ViewBag.BeddingTypeId = new SelectList(db.BeddingTypes, "BeddingTypeId", "BeddingTypeName", booking.BeddingTypeId);
            ViewBag.ChildId = new SelectList(db.Children, "ChildId", "ChildId", booking.ChildId);
            ViewBag.RoomQuantityId = new SelectList(db.RoomQuantities, "RoomQuantityId", "RoomQuantityId", booking.RoomQuantityId);
            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "RoomTypeId", "RoomTypeName", booking.RoomTypeId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdultId = new SelectList(db.Adults, "AdultId", "AdultId", booking.AdultId);
            ViewBag.BeddingTypeId = new SelectList(db.BeddingTypes, "BeddingTypeId", "BeddingTypeName", booking.BeddingTypeId);
            ViewBag.ChildId = new SelectList(db.Children, "ChildId", "ChildId", booking.ChildId);
            ViewBag.RoomQuantityId = new SelectList(db.RoomQuantities, "RoomQuantityId", "RoomQuantityId", booking.RoomQuantityId);
            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "RoomTypeId", "RoomTypeName", booking.RoomTypeId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingId,FirstName,LastName,Address,Email,Phone,NID_No,ArrivalTime,DepartureDate,AdultId,ChildId,RoomTypeId,BeddingTypeId,RoomQuantityId,Notes")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdultId = new SelectList(db.Adults, "AdultId", "AdultId", booking.AdultId);
            ViewBag.BeddingTypeId = new SelectList(db.BeddingTypes, "BeddingTypeId", "BeddingTypeName", booking.BeddingTypeId);
            ViewBag.ChildId = new SelectList(db.Children, "ChildId", "ChildId", booking.ChildId);
            ViewBag.RoomQuantityId = new SelectList(db.RoomQuantities, "RoomQuantityId", "RoomQuantityId", booking.RoomQuantityId);
            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "RoomTypeId", "RoomTypeName", booking.RoomTypeId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
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
