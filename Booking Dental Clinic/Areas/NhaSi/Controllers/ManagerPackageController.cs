using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Booking_Dental_Clinic.Models;

namespace Booking_Dental_Clinic.Areas.NhaSi.Controllers
{
    public class ManagerPackageController : Controller
    {
        private DentalClinicEntities db = new DentalClinicEntities();

        // GET: NhaSi/ManagerPackage
        public ActionResult Index()
        {
            var hoaDons = db.HoaDons.Include(h => h.AspNetUser).Include(h => h.GoiDichVu).Include(h => h.HinhThucThanhToan);
            return View(hoaDons.ToList());
        }

        // GET: NhaSi/ManagerPackage/Details/5
        public ActionResult Details(long? id)
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

        // GET: NhaSi/ManagerPackage/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.Id_GoiDichVu = new SelectList(db.GoiDichVus, "Id_DichVu", "TenDichVu");
            ViewBag.ID_ThanhToan = new SelectList(db.HinhThucThanhToans, "ID_ThanhToan", "TenHinhThuc");
            return View();
        }

        // POST: NhaSi/ManagerPackage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_HoaDon,TongTien,Id,Id_GoiDichVu,ID_ThanhToan")] HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                db.HoaDons.Add(hoaDon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", hoaDon.Id);
            ViewBag.Id_GoiDichVu = new SelectList(db.GoiDichVus, "Id_DichVu", "TenDichVu", hoaDon.Id_GoiDichVu);
            ViewBag.ID_ThanhToan = new SelectList(db.HinhThucThanhToans, "ID_ThanhToan", "TenHinhThuc", hoaDon.ID_ThanhToan);
            return View(hoaDon);
        }

        // GET: NhaSi/ManagerPackage/Edit/5
        public ActionResult Edit(long? id)
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
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", hoaDon.Id);
            ViewBag.Id_GoiDichVu = new SelectList(db.GoiDichVus, "Id_DichVu", "TenDichVu", hoaDon.Id_GoiDichVu);
            ViewBag.ID_ThanhToan = new SelectList(db.HinhThucThanhToans, "ID_ThanhToan", "TenHinhThuc", hoaDon.ID_ThanhToan);
            return View(hoaDon);
        }

        // POST: NhaSi/ManagerPackage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_HoaDon,TongTien,Id,Id_GoiDichVu,ID_ThanhToan")] HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hoaDon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", hoaDon.Id);
            ViewBag.Id_GoiDichVu = new SelectList(db.GoiDichVus, "Id_DichVu", "TenDichVu", hoaDon.Id_GoiDichVu);
            ViewBag.ID_ThanhToan = new SelectList(db.HinhThucThanhToans, "ID_ThanhToan", "TenHinhThuc", hoaDon.ID_ThanhToan);
            return View(hoaDon);
        }

        // GET: NhaSi/ManagerPackage/Delete/5
        public ActionResult Delete(long? id)
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

        // POST: NhaSi/ManagerPackage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
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
