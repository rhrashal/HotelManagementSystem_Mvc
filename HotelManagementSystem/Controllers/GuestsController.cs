using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HotelManagementSystem.Models;
using PagedList;

namespace HotelManagementSystem.Controllers
{
    
    public class GuestsController : Controller
    {
        private HotelManagementDB_DataEntities db = new HotelManagementDB_DataEntities();

        // GET: Guests
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            

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

            //*******************************
            var guestList = from s in db.Guests select s;
            if (!String.IsNullOrEmpty(searchString))
            { 
                guestList = guestList.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper()) || s.FirstName.ToUpper().Contains(searchString.ToUpper()));
            }
            //******************************
            switch (sortOrder)
            {
                case "name_desc":
                    guestList = guestList.OrderByDescending(s => s.FirstName);
                    break;                
                default:
                    guestList = guestList.OrderBy(s => s.FirstName);
                    break;
            }

            //return View(guestList.ToList());


            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(guestList.ToPagedList(pageNumber, pageSize));

        }


        
        public ActionResult MultuDataInsert()
        {
            return View();
        }

        [HttpPost]
        public JsonResult MultuDataInsert(string guestJson)
        {
            var js = new JavaScriptSerializer();

           Guest[] guest = js.Deserialize<Guest[]>(guestJson);

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Guests.AddRange(guest);
                    db.SaveChanges();
                    dbContextTransaction.Commit();
                    return Json("All Rows Inseted Into Table.");
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return Json("There is a Probem arise");
                }


            }

        }


        // GET: Guests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = db.Guests.Find(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            return View(guest);
        }

        // GET: Guests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Guests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GuestId,FirstName,LastName,Address,Email,Phone,NID_No")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                db.Guests.Add(guest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(guest);
        }

        // GET: Guests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = db.Guests.Find(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            return View(guest);
        }

        // POST: Guests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GuestId,FirstName,LastName,Address,Email,Phone,NID_No")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(guest);
        }

        // GET: Guests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = db.Guests.Find(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            return View(guest);
        }

        // POST: Guests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Guest guest = db.Guests.Find(id);
            db.Guests.Remove(guest);
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
