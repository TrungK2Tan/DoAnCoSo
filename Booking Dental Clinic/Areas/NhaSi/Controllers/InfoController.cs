using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Booking_Dental_Clinic.Models;
using Microsoft.AspNet.Identity;

namespace Booking_Dental_Clinic.Areas.NhaSi.Controllers
{
    public class InfoController : Controller
    {
        private DentalClinicEntities db = new DentalClinicEntities();

        // GET: NhaSi/Info
        public ActionResult Index()
        {
            // Get the logged-in user's ID
            string userId = User.Identity.GetUserId();

            // Retrieve the NhaSi object associated with the logged-in user
            Models.NhaSi nhaSi = db.NhaSis.FirstOrDefault(n => n.UserId == userId);

            // Pass the NhaSi object to the view
            return View(nhaSi);
        }


        // GET: NhaSi/Info/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.NhaSi nhaSi = db.NhaSis.Find(id);
            if (nhaSi == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", nhaSi.UserId);
            return View(nhaSi);
        }

        // POST: NhaSi/Info/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDBACSI,Ten,HinhAnh,ThongTin,UserId,GioBatDau,GioKetThuc")] Models.NhaSi nhaSi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhaSi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", nhaSi.UserId);
            return View(nhaSi);
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
