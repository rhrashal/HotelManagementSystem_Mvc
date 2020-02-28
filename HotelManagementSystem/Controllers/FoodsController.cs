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
    public class FoodsController : Controller
    {
        private HotelManagementDB_DataEntities db = new HotelManagementDB_DataEntities();

        // GET: Foods
        
        public ActionResult Index()
        {
            return View(db.Foods.ToList());
        }
        [AllowAnonymous]
        public ActionResult addCard()
        {
            return View(db.Foods.ToList());
        }

        // GET: Foods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Foods.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // GET: Foods/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Foods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FoodId,FoodName,ImageUrl,Price")] Food food, HttpPostedFileBase ImageFileCreate)
        {
            if (ModelState.IsValid)
            {
                ImageFileCreate.SaveAs(Server.MapPath("~/Images/Food") + "/" + ImageFileCreate.FileName);
                string filePath = "~/Images/Food/" + ImageFileCreate.FileName;
                food.ImageUrl = filePath;
                db.Foods.Add(food);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(food);
        }

        // GET: Foods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Foods.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FoodId,FoodName,ImageUrl,Price")] Food food, HttpPostedFileBase ImageFileCreate)
        {
            if (ModelState.IsValid)
            {
                if(ImageFileCreate != null)
                {
                    System.IO.File.Delete(Server.MapPath(food.ImageUrl));
                    ImageFileCreate.SaveAs(Server.MapPath("~/Images/Food") + "/" + ImageFileCreate.FileName);
                    string filePath = "~/Images/Food/" + ImageFileCreate.FileName;
                    food.ImageUrl = filePath;
                }
                
                db.Entry(food).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(food);
        }

        // GET: Foods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Foods.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Food food = db.Foods.Find(id);
            System.IO.File.Delete(Server.MapPath(food.ImageUrl));
            db.Foods.Remove(food);
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
