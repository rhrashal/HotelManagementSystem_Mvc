using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelManagementSystem.Models;
using PagedList;

namespace HotelManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReservasionsController : Controller
    {
        private HotelManagementDB_DataEntities db = new HotelManagementDB_DataEntities();

        // GET: Reservasions
        //public ActionResult Index1()
        //{
        //    var reservasions = db.Reservasions.Include(r => r.Adult).Include(r => r.Child).Include(r => r.Guest).Include(r => r.Room).Include(r => r.RoomQuantity);
        //    return View(reservasions.ToList());
        //}
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            //***For paging************
            ViewBag.CurrentSort = sortOrder;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            //*****************

            var reservasions =from s in db.Reservasions.Include(r => r.Adult).Include(r => r.Child).Include(r => r.Guest).Include(r => r.Room).Include(r => r.RoomQuantity) select s;

            //***for search ****************
            if (!String.IsNullOrEmpty(searchString))
            {
                reservasions = reservasions.Where(s =>s.Guest.FirstName.ToUpper().Contains(searchString.ToUpper())
                ||
                s.Guest.LastName.ToUpper().Contains(searchString.ToUpper()));
            }
            //************

            switch (sortOrder)
            {
                case "name_desc":
                    reservasions = reservasions.OrderByDescending(s => s.Guest.FirstName);
                    break;
                case "Date":
                    reservasions = reservasions.OrderBy(s => s.DepartureDate);
                    break;
                case "date_desc":
                    reservasions = reservasions.OrderByDescending(s => s.DepartureDate);
                    break;
                default:
                    reservasions = reservasions.OrderBy(s => s.Guest.FirstName);
                    break;
            }

            // for paging **********************
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(reservasions.ToPagedList(pageNumber, pageSize));

            //return View(reservasions.ToList());
        }

        // GET: Reservasions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservasion reservasion = db.Reservasions.Find(id);
            if (reservasion == null)
            {
                return HttpNotFound();
            }
            return View(reservasion);
        }

        // GET: Reservasions/Create
        public ActionResult Create()
        {
            ViewBag.AdultId = new SelectList(db.Adults, "AdultId", "NumberOfAdults");
            ViewBag.ChildId = new SelectList(db.Children, "ChildId", "NumberOfChild");
            ViewBag.GuestId = new SelectList(db.Guests, "GuestId", "FullName");
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomTypeName");
            ViewBag.RoomQuantityId = new SelectList(db.RoomQuantities, "RoomQuantityId", "NumberOfRoom");
            return View();
        }

        // POST: Reservasions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReservasionId,GuestId,RoomId,AdultId,ChildId,RoomQuantityId,ArrivalTime,DepartureDate")] Reservasion reservasion)
        {
            if (ModelState.IsValid)
            {
                db.Reservasions.Add(reservasion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdultId = new SelectList(db.Adults, "AdultId", "AdultId", reservasion.AdultId);
            ViewBag.ChildId = new SelectList(db.Children, "ChildId", "ChildId", reservasion.ChildId);
            ViewBag.GuestId = new SelectList(db.Guests, "GuestId", "FirstName", reservasion.GuestId);
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomTypeName", reservasion.RoomId);
            ViewBag.RoomQuantityId = new SelectList(db.RoomQuantities, "RoomQuantityId", "RoomQuantityId", reservasion.RoomQuantityId);
            return View(reservasion);
        }

        // GET: Reservasions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservasion reservasion = db.Reservasions.Find(id);
            if (reservasion == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdultId = new SelectList(db.Adults, "AdultId", "AdultId", reservasion.AdultId);
            ViewBag.ChildId = new SelectList(db.Children, "ChildId", "ChildId", reservasion.ChildId);
            ViewBag.GuestId = new SelectList(db.Guests, "GuestId", "FirstName", reservasion.GuestId);
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomTypeName", reservasion.RoomId);
            ViewBag.RoomQuantityId = new SelectList(db.RoomQuantities, "RoomQuantityId", "RoomQuantityId", reservasion.RoomQuantityId);
            return View(reservasion);
        }

        // POST: Reservasions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReservasionId,GuestId,RoomId,AdultId,ChildId,RoomQuantityId,ArrivalTime,DepartureDate")] Reservasion reservasion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservasion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdultId = new SelectList(db.Adults, "AdultId", "AdultId", reservasion.AdultId);
            ViewBag.ChildId = new SelectList(db.Children, "ChildId", "ChildId", reservasion.ChildId);
            ViewBag.GuestId = new SelectList(db.Guests, "GuestId", "FirstName", reservasion.GuestId);
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomTypeName", reservasion.RoomId);
            ViewBag.RoomQuantityId = new SelectList(db.RoomQuantities, "RoomQuantityId", "RoomQuantityId", reservasion.RoomQuantityId);
            return View(reservasion);
        }

        // GET: Reservasions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservasion reservasion = db.Reservasions.Find(id);
            if (reservasion == null)
            {
                return HttpNotFound();
            }
            return View(reservasion);
        }

        // POST: Reservasions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservasion reservasion = db.Reservasions.Find(id);
            db.Reservasions.Remove(reservasion);
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
