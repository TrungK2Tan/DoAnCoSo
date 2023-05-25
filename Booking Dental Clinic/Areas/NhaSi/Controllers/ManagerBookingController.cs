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
    public class ManagerBookingController : Controller
    {
        private DentalClinicEntities db = new DentalClinicEntities();

        // GET: NhaSi/ManagerBooking
        public ActionResult Index()
        {
            // ID loại nha sĩ cần lấy lịch hẹn
            var loaiNhaSiId = User.Identity.GetUserId(); // Thay thế 1 bằng ID loại nha sĩ cần lấy

            // Lấy lịch hẹn theo ID loại nha sĩ
            var lichHens = db.LichHens.Include(l => l.AspNetUser)
                .Include(l => l.LoaiDichVu)
                .Include(l => l.NhaSi)
                .Where(l => l.NhaSi.UserId == loaiNhaSiId); // Áp dụng điều kiện ID loại nha sĩ

            return View(lichHens.ToList());
            //var lichHens = db.LichHens.Include(l => l.AspNetUser).Include(l => l.LoaiDichVu).Include(l => l.NhaSi);
            //return View(lichHens.ToList());
        }

        // GET: NhaSi/ManagerBooking/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichHen lichHen = db.LichHens.Find(id);
            if (lichHen == null)
            {
                return HttpNotFound();
            }
            return View(lichHen);
        }

        // GET: NhaSi/ManagerBooking/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.IDDICHVU = new SelectList(db.LoaiDichVus, "IDDICHVU", "Ten");
            ViewBag.IDBACSI = new SelectList(db.NhaSis, "IDBACSI", "Ten");
            return View();
        }

        // POST: NhaSi/ManagerBooking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdLich,Tenkhachhang,IDBACSI,IDDICHVU,Sdt,Ngaydat,Id,Gio")] LichHen lichHen)
        {
            if (ModelState.IsValid)
            {
                db.LichHens.Add(lichHen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", lichHen.Id);
            ViewBag.IDDICHVU = new SelectList(db.LoaiDichVus, "IDDICHVU", "Ten", lichHen.IDDICHVU);
            ViewBag.IDBACSI = new SelectList(db.NhaSis, "IDBACSI", "Ten", lichHen.IDBACSI);
            return View(lichHen);
        }

        // GET: NhaSi/ManagerBooking/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichHen lichHen = db.LichHens.Find(id);
            if (lichHen == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", lichHen.Id);
            ViewBag.IDDICHVU = new SelectList(db.LoaiDichVus, "IDDICHVU", "Ten", lichHen.IDDICHVU);
            ViewBag.IDBACSI = new SelectList(db.NhaSis, "IDBACSI", "Ten", lichHen.IDBACSI);
            return View(lichHen);
        }

        // POST: NhaSi/ManagerBooking/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdLich,Tenkhachhang,IDBACSI,IDDICHVU,Sdt,Ngaydat,Id,Gio")] LichHen lichHen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lichHen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", lichHen.Id);
            ViewBag.IDDICHVU = new SelectList(db.LoaiDichVus, "IDDICHVU", "Ten", lichHen.IDDICHVU);
            ViewBag.IDBACSI = new SelectList(db.NhaSis, "IDBACSI", "Ten", lichHen.IDBACSI);
            return View(lichHen);
        }

        // GET: NhaSi/ManagerBooking/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichHen lichHen = db.LichHens.Find(id);
            if (lichHen == null)
            {
                return HttpNotFound();
            }
            return View(lichHen);
        }

        // POST: NhaSi/ManagerBooking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LichHen lichHen = db.LichHens.Find(id);
            db.LichHens.Remove(lichHen);
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
