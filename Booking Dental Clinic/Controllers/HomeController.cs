using Booking_Dental_Clinic.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using MoMo;
using Newtonsoft.Json.Linq;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace Booking_Dental_Clinic.Controllers
{
    public class HomeController : Controller
    {
         DentalClinicEntities db = new DentalClinicEntities();
        // GET: Home
        public ActionResult Index()
        { 
            List<NhaSi> nhasi = db.NhaSis.ToList();
            
            return View(nhasi);
        }
        [Authorize]
        public ActionResult BookingView()
        {

            // Lấy ID đăng nhập của người dùng hiện tại
            string currentUserId = User.Identity.GetUserId();
            // Lấy danh sách lịch hẹn dựa trên ID đăng nhập
            var lichHens = db.LichHens
                .Where(l => l.Id == currentUserId)
                .ToList();
            return View(lichHens);
        }
        [Authorize]
        public ActionResult PriceView()
        {

            // Lấy ID đăng nhập của người dùng hiện tại
            string currentUserId = User.Identity.GetUserId();
            // Lấy danh sách dich vu dựa trên ID đăng nhập
            var dichvus = db.HoaDons
                .Where(l => l.Id == currentUserId)
                .ToList();
            return View(dichvus);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaSi dental = db.NhaSis.Find(id);
            if (dental == null)
            {
                return HttpNotFound();
            }
            return View(dental);
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Price()
        {
            // lay ra danh sach goi dich vu
            List<GoiDichVu> ketqua = db.GoiDichVus.ToList();
            return View(ketqua);
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Service()
        {
            return View();
        }
        public ActionResult BlogGrid(int? page)
        {
            if (page == null) page = 1;
            var dienDans = db.DienDans.OrderBy(b => b.Id_diendan);
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(dienDans.ToPagedList(pageNumber, pageSize));
            //List<DienDan> diendan = db.DienDans.ToList();
            //return View(diendan);
        }
        public ActionResult BlogDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DienDan diendan = db.DienDans.Find(id);
            if (diendan == null)
            {
                return HttpNotFound();
            }
            return View(diendan);
        }
        public ActionResult Team(int? page)
        {
            if (page == null) page = 1;
            var nhasis = db.NhaSis.OrderBy(b => b.IDBACSI);
            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(nhasis.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Testimonial()
        {
            return View();
        }
        [Authorize]
        public ActionResult Appointment(string ten, string sdt, int? IDBACSI, string GioBatDau, string GioKetThuc)
        {
            var nhasi = db.NhaSis.FirstOrDefault(c => c.IDBACSI == IDBACSI);
            var a = User.Identity.GetUserName();
            ViewBag.Ten = ten;
            ViewBag.Sdt = sdt;
            ViewBag.IDBACSI = new SelectList(db.NhaSis, "IDBACSI", "Ten");
            ViewBag.IDDICHVU = new SelectList(db.LoaiDichVus, "IDDICHVU", "Ten");
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Id");
            ViewBag.a = a;
            ViewBag.IDBACSI = IDBACSI;
            ViewBag.GioBatDau = GioBatDau;
            ViewBag.GioKetThuc = GioKetThuc;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Appointment([Bind(Include = "IdLich,Tenkhachhang,IDDICHVU,IDBACSI,Sdt,Ngaydat,Id,GioBatDau,GioKetThuc")] LichHen DatLich)
        {
            if (ModelState.IsValid)
            {
                // Lấy ID của người dùng hiện tại
                string currentUserId = User.Identity.GetUserId();

                // Gán ID vào thuộc tính DatLich.Id
                DatLich.Id = currentUserId;
                // Kiểm tra số lượng lịch hẹn đã đặt cho nha sĩ tương ứng
                var numberOfAppointments = db.LichHens.Count(l => l.IDBACSI == DatLich.IDBACSI && l.GioBatDau == DatLich.GioBatDau && l.GioKetThuc == DatLich.GioKetThuc);
                if (numberOfAppointments >= 2)
                {
                    ModelState.AddModelError("", "Nha sĩ này đã đủ số lượng lịch hẹn cho khung giờ này. Vui lòng chọn nha sĩ khác.");
                    // Nếu muốn chuyển đến một trang thông báo hoặc hiển thị thông báo trong view, bạn có thể thực hiện ở đây.
                    // Ví dụ: return RedirectToAction("AppointmentFull");
                    // hoặc ViewBag.AppointmentFull = true;
                    ViewBag.IDDICHVU = new SelectList(db.LoaiDichVus, "IDDICHVU", "Ten", DatLich.IDDICHVU);
                    ViewBag.Id = DatLich.Id;
                    return View(DatLich);
                }
                db.LichHens.Add(DatLich);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.IDBACSI = new SelectList(db.NhaSis, "IDBACSI", "Ten", DatLich.IDBACSI);
            ViewBag.IDDICHVU = new SelectList(db.LoaiDichVus, "IDDICHVU", "Ten", DatLich.IDDICHVU);
            ViewBag.Id = DatLich.Id;
            return View(DatLich);
        }
        public ActionResult Search()
        {
            return View();
        }
        public ActionResult Thanhtoan(int? Id_DichVu)
        {
            var a = User.Identity.GetUserId();
            var dichVu = db.GoiDichVus.FirstOrDefault(p => p.Id_DichVu == Id_DichVu);
            var tienDichvu = dichVu.GiaDichVu;
            if (a == null )
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Id_GoiDichVu = new SelectList(db.GoiDichVus, "Id_DichVu", "TenDichVu");
            ViewBag.ID_ThanhToan = new SelectList(db.HinhThucThanhToans, "ID_ThanhToan", "TenHinhThuc");
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "UserName");
            ViewBag.IDBACSI = new SelectList(db.NhaSis, "IDBACSI", "Ten");
            ViewBag.Id_DichVu = Id_DichVu;
            ViewBag.GiaDichVu = tienDichvu;
            ViewBag.a = a;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Thanhtoan([Bind(Include = "ID_HoaDon,TongTien,Id,Id_GoiDichVu,ID_ThanhToan,IDBACSI")] HoaDon HoaDon)
        {
            {
                if (ModelState.IsValid)
                {
                    // Lấy ID của người dùng hiện tại
                    string currentUserId = User.Identity.GetUserId();

                    // Gán ID vào thuộc tính HoaDon.Id
                    HoaDon.ID_HoaDon = Guid.NewGuid().ToString();
                    HoaDon.Id = currentUserId;
                    HoaDon.TrangThai = false;
                    db.HoaDons.Add(HoaDon);
                    db.SaveChanges();
                    if(HoaDon.ID_ThanhToan == 2)
                    {
                        return RedirectToAction("Payment", "Home", new { order = HoaDon.ID_HoaDon });
                    }
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Id_GoiDichVu = new SelectList(db.GoiDichVus, "Id_DichVu", "TenDichVu", HoaDon.Id_GoiDichVu);
                ViewBag.ID_ThanhToan = new SelectList(db.HinhThucThanhToans, "ID_ThanhToan", "TenHinhThuc",HoaDon.ID_ThanhToan);
                ViewBag.IDBACSI = new SelectList(db.NhaSis, "IDBACSI", "Ten", HoaDon.IDBACSI);
                ViewBag.Id = HoaDon.Id;
                return View(HoaDon);
            }
        }
        public ActionResult Payment(string order)
        {
            string userId = User.Identity.GetUserId();
            var username = db.AspNetUsers.FirstOrDefault(u => u.Id == userId).FullName;
            var hd = db.HoaDons.FirstOrDefault(u => u.ID_HoaDon == order);
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo =  username + "Thanh toán đơn hàng";
            string returnUrl = "https://localhost:44390/Home/ConfirmPaymentClient";
            string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = hd.TongTien.ToString();
            string orderid = order.ToString(); //mã đơn hàng
            string requestId = order.ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);

            return Redirect(jmessage.GetValue("payUrl").ToString());
        }
        public ActionResult ConfirmPaymentClient(Result result)
        {

            if (result != null && result.errorCode == "0")
            {
                var hd = db.HoaDons.FirstOrDefault(u => u.ID_HoaDon == result.orderId);
                hd.TrangThai = true;
                db.HoaDons.AddOrUpdate(hd);
                db.SaveChanges();
                ViewBag.Noification = "Thanh toán thành công";
                return View();
            }
            ViewBag.Noification = "Thanh toán lỗi";
            return View();

        }


    }
}