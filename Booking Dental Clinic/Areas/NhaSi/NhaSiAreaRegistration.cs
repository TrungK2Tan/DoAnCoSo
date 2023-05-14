﻿using System.Web.Mvc;

namespace Booking_Dental_Clinic.Areas.NhaSi
{
    public class NhaSiAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "NhaSi";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "NhaSi_default",
                "NhaSi/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}