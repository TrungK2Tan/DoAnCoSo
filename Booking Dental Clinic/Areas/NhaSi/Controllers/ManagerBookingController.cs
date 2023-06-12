using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Booking_Dental_Clinic.Models;
using EllipticCurve.Utils;
using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;


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
                .Where(l => l.NhaSi.UserId == loaiNhaSiId).OrderBy(l => l.Ngaydat);  // Áp dụng điều kiện ID loại nha sĩ

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
        //public async Task<ActionResult> Approve(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    LichHen lichHen = db.LichHens.Find(id);
        //    if (lichHen == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    lichHen.IsApproved = true; // Update the IsApproved property to true
        //    await db.SaveChangesAsync();

        //    return RedirectToAction("Index");
        //}

        public async Task<ActionResult> Approve(int? id)
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

            lichHen.IsApproved = true; // Cập nhật thuộc tính IsApproved thành true
            await db.SaveChangesAsync();

            // Lấy ngày giờ đặt lịch
            DateTime appointmentDate = lichHen.Ngaydat?.Date ?? DateTime.MinValue;
            DateTime startTime = appointmentDate + lichHen.GioBatDau.Value;
            DateTime endTime = appointmentDate + lichHen.GioKetThuc.Value;


            // Nội dung email 
            string emailTo = lichHen.AspNetUser.Email; // Email address of the appointment holder
            string subject = "Thông báo đặt lịch hẹn thành công";
            string body = $"Lịch hẹn của bạn đã được chấp nhận thành công.\n" +
                  $"Thông tin lịch hẹn:\n" +
                  $"- Nha sĩ: {lichHen.NhaSi.Ten}\n" +
                  $"- Ngày: {appointmentDate.ToShortDateString()}\n" +
                  $"- Giờ bắt đầu: {startTime.ToShortTimeString()}\n" +
                  $"- Giờ kết thúc: {endTime.ToShortTimeString()}";

            // Configure SendGrid information
            string sendGridApiKey = "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";
            string sendGridFromEmail = "nhakhoasth@gmail.com";
            string sendGridFromName = "Nha Khoa STH";

            // Tạo đối tượng SendGridMessage
            SendGridMessage sendGridMessage = new SendGridMessage();
            sendGridMessage.From = new EmailAddress(sendGridFromEmail, sendGridFromName);
            sendGridMessage.AddTo(emailTo);
            sendGridMessage.Subject = subject;
            sendGridMessage.PlainTextContent = body;
            sendGridMessage.HtmlContent = body;

            // Tạo đối tượng SendGridClient và gửi email
            var sendGridClient = new SendGridClient(sendGridApiKey);

            try
            {
                var response = await sendGridClient.SendEmailAsync(sendGridMessage);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu gửi email không thành công
                // ...
            }

            return RedirectToAction("Index");
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
