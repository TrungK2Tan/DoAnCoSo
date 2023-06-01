using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Booking_Dental_Clinic.Models
{
    public class SearchViewModel
    {
        public string Keyword { get; set; }
        public List<NhaSi> NhaSiResults { get; set; }
        public List<GoiDichVu> DichVuResults { get; set; }
    }
}