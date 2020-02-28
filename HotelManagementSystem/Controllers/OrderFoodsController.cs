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
    [Authorize]
    public class OrderFoodsController : Controller
    {
        private HotelManagementDB_DataEntities db = new HotelManagementDB_DataEntities();

        // GET: OrderFoods
        //public ActionResult Index()
        //{
        //    var orderFoods = db.OrderFoods.Include(o => o.Food).Include(o => o.Guest);
        //    return View(orderFoods.ToList());
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
            var orderFoods = from s in db.OrderFoods.Include(o => o.Food).Include(o => o.Guest) select s;
            

            //***for search ****************
            if (!String.IsNullOrEmpty(searchString))
            {
                orderFoods = orderFoods.Where(s => s.Guest.FirstName.ToUpper().Contains(searchString.ToUpper())
                ||  s.Guest.LastName.ToUpper().Contains(searchString.ToUpper()) || s.Food.FoodName.ToUpper().Contains(searchString.ToUpper())); 
            }
            //************

            switch (sortOrder)
            {
                case "name_desc":
                    orderFoods = orderFoods.OrderByDescending(s => s.Guest.FirstName);
                    break;
                case "Date":
                    orderFoods = orderFoods.OrderBy(s => s.Food.FoodName);
                    break;
                case "date_desc":
                    orderFoods = orderFoods.OrderByDescending(s => s.Food.FoodName);
                    break;
                default:
                    orderFoods = orderFoods.OrderBy(s => s.Guest.FirstName);
                    break;
            }

            // for paging **********************
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(orderFoods.ToPagedList(pageNumber, pageSize));


            
           // return View(orderFoods.ToList());
        }
        
        // GET: OrderFoods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderFood orderFood = db.OrderFoods.Find(id);
            if (orderFood == null)
            {
                return HttpNotFound();
            }
            return View(orderFood);
        }

        // GET: OrderFoods/Create
        public ActionResult Create()
        {
            ViewBag.FoodId = new SelectList(db.Foods, "FoodId", "FoodName");
            ViewBag.GuestId = new SelectList(db.Guests, "GuestId", "FullName");
            return View();
        }

        // POST: OrderFoods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,GuestId,FoodId,Quntity")] OrderFood orderFood)
        {
            if (ModelState.IsValid)
            {
                db.OrderFoods.Add(orderFood);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FoodId = new SelectList(db.Foods, "FoodId", "FoodName", orderFood.FoodId);
            ViewBag.GuestId = new SelectList(db.Guests, "GuestId", "FirstName", orderFood.GuestId);
            return View(orderFood);
        }

        // GET: OrderFoods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderFood orderFood = db.OrderFoods.Find(id);
            if (orderFood == null)
            {
                return HttpNotFound();
            }
            ViewBag.FoodId = new SelectList(db.Foods, "FoodId", "FoodName", orderFood.FoodId);
            ViewBag.GuestId = new SelectList(db.Guests, "GuestId", "FirstName", orderFood.GuestId);
            return View(orderFood);
        }

        // POST: OrderFoods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,GuestId,FoodId,Quntity")] OrderFood orderFood)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderFood).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FoodId = new SelectList(db.Foods, "FoodId", "FoodName", orderFood.FoodId);
            ViewBag.GuestId = new SelectList(db.Guests, "GuestId", "FirstName", orderFood.GuestId);
            return View(orderFood);
        }

        // GET: OrderFoods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderFood orderFood = db.OrderFoods.Find(id);
            if (orderFood == null)
            {
                return HttpNotFound();
            }
            return View(orderFood);
        }

        // POST: OrderFoods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderFood orderFood = db.OrderFoods.Find(id);
            db.OrderFoods.Remove(orderFood);
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


        public ActionResult getRelatedGuest(string searchString, int? GuestId, int? OrderId)
        {
            var geustFoodOrder = new GeustFoodOrder();
            geustFoodOrder.Guests = db.Guests.ToList();

            if (GuestId == null)
            {
                if (Session["GuestId"] != null)
                {
                    GuestId = Convert.ToInt32(Session["GuestId"].ToString());
                }
            }

            if (GuestId != null)
            {

                Session["GuestId"] = GuestId;
                geustFoodOrder.OrderFoods = db.OrderFoods.Where(w => w.GuestId == GuestId.Value).ToList();


            }

            if (OrderId != null)
            {

                geustFoodOrder.Foods = db.Foods.Where(w => w.FoodId == OrderId.Value).ToList();

            }

            return View(geustFoodOrder);
        }
    }
}
