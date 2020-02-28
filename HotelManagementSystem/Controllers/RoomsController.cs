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
    public class RoomsController : Controller
    {
        private HotelManagementDB_DataEntities db = new HotelManagementDB_DataEntities();

        // GET: Rooms
        public ActionResult Index()
        {
            var rooms = db.Rooms.Include(r => r.BeddingType).Include(r => r.RoomType);
            return View(rooms.ToList());
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            ViewBag.BeddingTypeId = new SelectList(db.BeddingTypes, "BeddingTypeId", "BeddingTypeName");
            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "RoomTypeId", "RoomTypeName");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomId,RoomTypeName,RoomTypeId,BeddingTypeId,ImageUrl,Price")] Room room, HttpPostedFileBase ImageFileCreate)
        {
            if (ModelState.IsValid)
            {
                ImageFileCreate.SaveAs(Server.MapPath("~/Images/Room") + "/" + ImageFileCreate.FileName);
                string filePath = "~/Images/Room/" + ImageFileCreate.FileName;
                room.ImageUrl = filePath;
                db.Rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BeddingTypeId = new SelectList(db.BeddingTypes, "BeddingTypeId", "BeddingTypeName", room.BeddingTypeId);
            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "RoomTypeId", "RoomTypeName", room.RoomTypeId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            ViewBag.BeddingTypeId = new SelectList(db.BeddingTypes, "BeddingTypeId", "BeddingTypeName", room.BeddingTypeId);
            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "RoomTypeId", "RoomTypeName", room.RoomTypeId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomId,RoomTypeName,RoomTypeId,BeddingTypeId,ImageUrl,Price")] Room room, HttpPostedFileBase ImageFileCreate)
        {
            if (ModelState.IsValid)
            {
                if(ImageFileCreate != null)
                {
                    System.IO.File.Delete(Server.MapPath(room.ImageUrl));
                    ImageFileCreate.SaveAs(Server.MapPath("~/Images/Room") + "/" + ImageFileCreate.FileName);
                    string filePath = "~/Images/Room/" + ImageFileCreate.FileName;
                    room.ImageUrl = filePath;
                }
                

                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BeddingTypeId = new SelectList(db.BeddingTypes, "BeddingTypeId", "BeddingTypeName", room.BeddingTypeId);
            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "RoomTypeId", "RoomTypeName", room.RoomTypeId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Find(id);
            System.IO.File.Delete(Server.MapPath(room.ImageUrl));
            db.Rooms.Remove(room);
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
        [AllowAnonymous]
        public ActionResult getRoom()
        {
            var rooms = db.Rooms.Include(r => r.BeddingType).Include(r => r.RoomType);
            return View(rooms.ToList());
        }
    }
}
