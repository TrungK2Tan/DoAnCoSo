using Booking_Dental_Clinic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Booking_Dental_Clinic.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private DentalClinicEntities db = new DentalClinicEntities(); 
        // GET: Admin/Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search()
        {
            return View();
        }
        public ActionResult TotalRevenue()
        {
            // Retrieve the list of HoaDons where TrangThai is true
            var hoaDons = db.HoaDons.Where(i => i.TrangThai == true).ToList();

            // Calculate the total revenue
            decimal? totalRevenue = hoaDons.Sum(i => i.TongTien);

            // Pass the list of HoaDons and totalRevenue to the view
            ViewBag.HoaDons = hoaDons;
            ViewBag.TotalRevenue = totalRevenue;

            return View();
        }



    }

}
