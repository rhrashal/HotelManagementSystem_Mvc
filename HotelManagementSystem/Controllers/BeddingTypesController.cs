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
    public class BeddingTypesController : Controller
    {
        private HotelManagementDB_DataEntities db = new HotelManagementDB_DataEntities();

        // GET: BeddingTypes
        public ActionResult Index()
        {
            return View(db.BeddingTypes.ToList());
        }

        // GET: BeddingTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BeddingType beddingType = db.BeddingTypes.Find(id);
            if (beddingType == null)
            {
                return HttpNotFound();
            }
            return View(beddingType);
        }

        // GET: BeddingTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BeddingTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BeddingTypeId,BeddingTypeName")] BeddingType beddingType)
        {
            if (ModelState.IsValid)
            {
                db.BeddingTypes.Add(beddingType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(beddingType);
        }

        // GET: BeddingTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BeddingType beddingType = db.BeddingTypes.Find(id);
            if (beddingType == null)
            {
                return HttpNotFound();
            }
            return View(beddingType);
        }

        // POST: BeddingTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BeddingTypeId,BeddingTypeName")] BeddingType beddingType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beddingType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beddingType);
        }

        // GET: BeddingTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BeddingType beddingType = db.BeddingTypes.Find(id);
            if (beddingType == null)
            {
                return HttpNotFound();
            }
            return View(beddingType);
        }

        // POST: BeddingTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BeddingType beddingType = db.BeddingTypes.Find(id);
            db.BeddingTypes.Remove(beddingType);
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
