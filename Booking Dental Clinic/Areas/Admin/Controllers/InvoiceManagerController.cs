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
    public class InvoiceManagerController : Controller
    {
        private DentalClinicEntities db = new DentalClinicEntities();

        // GET: Admin/InvoiceManager
        public ActionResult Index()
        {
            var hoaDons = db.HoaDons.Include(h => h.AspNetUser).Include(h => h.GoiDichVu).Include(h => h.HinhThucThanhToan).Include(h => h.NhaSi);
            return View(hoaDons.ToList());
        }

        // GET: Admin/InvoiceManager/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }
      
        // GET: Admin/InvoiceManager/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }

        // POST: Admin/InvoiceManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HoaDon hoaDon = db.HoaDons.Find(id);
            db.HoaDons.Remove(hoaDon);
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
