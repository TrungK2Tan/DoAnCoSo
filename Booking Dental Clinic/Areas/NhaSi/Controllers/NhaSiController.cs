using Booking_Dental_Clinic.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Booking_Dental_Clinic.Areas.NhaSi.Controllers
{
    [Authorize(Roles = "NhaSi")]
    public class NhaSiController : Controller
    {
        private DentalClinicEntities db = new DentalClinicEntities();

        // GET: NhaSi/NhaSi
        public ActionResult Index()
        {
            return View();
        }
    }
}