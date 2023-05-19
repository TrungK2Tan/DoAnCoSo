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
        public ActionResult BlogGrid()
        {
            List<DienDan> diendan = db.DienDans.ToList();
            return View(diendan);
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
        public ActionResult Appointment()
        {
            var a = User.Identity.GetUserName();
            //var user = db.AspNetUsers.Find(id);
            ViewBag.IDBACSI = new SelectList(db.NhaSis, "IDBACSI", "Ten");
            ViewBag.IDDICHVU = new SelectList(db.LoaiDichVus, "IDDICHVU", "Ten");
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Id");
            ViewBag.a = a;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Appointment([Bind(Include = "IdLich,Tenkhachhang,IDDICHVU,IDBACSI,Sdt,Ngaydat,Id")] LichHen DatLich)
        {
            if (ModelState.IsValid)
            {
                // Lấy ID của người dùng hiện tại
                string currentUserId = User.Identity.GetUserId();

                // Gán ID vào thuộc tính DatLich.Id
                DatLich.Id = currentUserId;
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
        public ActionResult Thanhtoan()
        {
            //if (Session["Id"] == null || Session["Id"].ToString() == "")
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            var a = User.Identity.GetUserId();
            if (a == null )
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Id_GoiDichVu = new SelectList(db.GoiDichVus, "Id_DichVu", "TenDichVu");
            ViewBag.ID_ThanhToan = new SelectList(db.HinhThucThanhToans, "ID_ThanhToan", "TenHinhThuc");
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "UserName");
            ViewBag.a = a;
            return View();


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Thanhtoan([Bind(Include = "ID_HoaDon,TongTien,Id,Id_GoiDichVu,ID_ThanhToan")] HoaDon HoaDon)
        {
            {
                if (ModelState.IsValid)
                {
                    // Lấy ID của người dùng hiện tại
                    string currentUserId = User.Identity.GetUserId();

                    // Gán ID vào thuộc tính HoaDon.Id
                    HoaDon.Id = currentUserId;
                    db.HoaDons.Add(HoaDon);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Id_GoiDichVu = new SelectList(db.GoiDichVus, "Id_DichVu", "TenDichVu", HoaDon.Id_GoiDichVu);
                ViewBag.ID_ThanhToan = new SelectList(db.HinhThucThanhToans, "ID_ThanhToan", "TenHinhThuc",HoaDon.ID_ThanhToan);
                ViewBag.Id = HoaDon.Id;
                return View(HoaDon);
            }
        }
        public ActionResult Payment()
        {
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = "test";
            string returnUrl = "https://localhost:44394/Home/Thanhtoan";
            string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = "1000";
            string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
            string requestId = DateTime.Now.Ticks.ToString();
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
    }
}