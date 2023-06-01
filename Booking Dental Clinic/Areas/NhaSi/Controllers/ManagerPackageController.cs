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
    public class ManagerPackageController : Controller
    {
        private DentalClinicEntities db = new DentalClinicEntities();

        // GET: NhaSi/ManagerPackage
        public ActionResult Index()
        {
            // ID loại nha sĩ cần lấy lịch hẹn
            var loaiNhaSiId = User.Identity.GetUserId(); // Thay thế 1 bằng ID loại nha sĩ cần lấy

            // Lấy lịch hẹn theo ID loại nha sĩ
            var hoaDons = db.HoaDons.Include(l => l.AspNetUser)
                .Include(l => l.GoiDichVu)
                .Include(l => l.NhaSi)
                .Where(l => l.NhaSi.UserId == loaiNhaSiId); // Áp dụng điều kiện ID loại nha sĩ

            return View(hoaDons.ToList());
            //var hoaDons = db.HoaDons.Include(h => h.AspNetUser).Include(h => h.GoiDichVu).Include(h => h.HinhThucThanhToan);
            //return View(hoaDons.ToList());
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
