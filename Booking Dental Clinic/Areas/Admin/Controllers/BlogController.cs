using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Booking_Dental_Clinic.Models;

namespace Booking_Dental_Clinic.Areas.Admin.Controllers
{
    public class BlogController : Controller
    {
        private DentalClinicEntities db = new DentalClinicEntities();

        // GET: Admin/Blog
        public ActionResult Index()
        {
            return View(db.DienDans.ToList());
        }

        // GET: Admin/Blog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DienDan dienDan = db.DienDans.Find(id);
            if (dienDan == null)
            {
                return HttpNotFound();
            }
            return View(dienDan);
        }

        // GET: Admin/Blog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Blog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_diendan,title,img,content,img2,content2")] DienDan dienDan)
        {
            if (ModelState.IsValid)
            {
                db.DienDans.Add(dienDan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dienDan);
        }

        // GET: Admin/Blog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DienDan dienDan = db.DienDans.Find(id);
            if (dienDan == null)
            {
                return HttpNotFound();
            }
            return View(dienDan);
        }

        // POST: Admin/Blog/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_diendan,title,img,content,img2,content2")] DienDan dienDan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dienDan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dienDan);
        }

        // GET: Admin/Blog/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DienDan dienDan = db.DienDans.Find(id);
            if (dienDan == null)
            {
                return HttpNotFound();
            }
            return View(dienDan);
        }

        // POST: Admin/Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DienDan dienDan = db.DienDans.Find(id);
            db.DienDans.Remove(dienDan);
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
