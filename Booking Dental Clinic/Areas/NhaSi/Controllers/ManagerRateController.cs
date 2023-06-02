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
    public class ManagerRateController : Controller
    {
        private DentalClinicEntities db = new DentalClinicEntities();

        // GET: NhaSi/ManagerRate
        public ActionResult Index()
        {
            // ID loại nha sĩ cần lấy đánh giá
            var loaiNhaSiId = User.Identity.GetUserId(); // Thay thế 1 bằng ID loại nha sĩ cần lấy
            var danhGiaBinhLuans = db.DanhGiaBinhLuans.Include(d => d.NhaSi).Where(l => l.NhaSi.UserId == loaiNhaSiId); ;
            return View(danhGiaBinhLuans.ToList());
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
