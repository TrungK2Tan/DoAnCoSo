using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Booking_Dental_Clinic.Models;
using PagedList;

namespace Booking_Dental_Clinic.Areas.Admin.Controllers
{
    public class InvoiceManagerController : Controller
    {
        private DentalClinicEntities db = new DentalClinicEntities();

        // GET: Admin/InvoiceManager
        public ActionResult Index(int? page)
        {
            int pageSize = 5; // Số lượng hóa đơn hiển thị trên mỗi trang
            int pageNumber = (page ?? 1); // Số trang hiện tại

            var hoaDons = db.HoaDons.Include(h => h.AspNetUser).Include(h => h.GoiDichVu).Include(h => h.HinhThucThanhToan).Include(h => h.NhaSi)
                            .OrderByDescending(h => h.ID_HoaDon); // Sắp xếp danh sách hóa đơn theo ID giảm dần

            var pagedHoaDons = hoaDons.ToPagedList(pageNumber, pageSize);

            return View(pagedHoaDons);
        }
        public ActionResult Edit(string id)
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
            ViewBag.IDBACSI = new SelectList(db.NhaSis, "IDBACSI", "Ten", hoaDon.IDBACSI);
            return View(hoaDon);
        }

        // POST: Admin/HoaDons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_HoaDon,TongTien,Id,Id_GoiDichVu,ID_ThanhToan,TrangThai,IDBACSI")] HoaDon hoaDon)
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
            ViewBag.IDBACSI = new SelectList(db.NhaSis, "IDBACSI", "Ten", hoaDon.IDBACSI);
            return View(hoaDon);
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
