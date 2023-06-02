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
    public class RateController : Controller
    {
        private DentalClinicEntities db = new DentalClinicEntities();

        // GET: Admin/Rate
        public ActionResult Index()
        {
            var danhGiaBinhLuans = db.DanhGiaBinhLuans.Include(d => d.NhaSi);
            return View(danhGiaBinhLuans.ToList());
        }
        // GET: Admin/Rate/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhGiaBinhLuan danhGiaBinhLuan = db.DanhGiaBinhLuans.Find(id);
            if (danhGiaBinhLuan == null)
            {
                return HttpNotFound();
            }
            return View(danhGiaBinhLuan);
        }

        // POST: Admin/Rate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            DanhGiaBinhLuan danhGiaBinhLuan = db.DanhGiaBinhLuans.Find(id);
            db.DanhGiaBinhLuans.Remove(danhGiaBinhLuan);
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
