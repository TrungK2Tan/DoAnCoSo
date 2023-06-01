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
using Microsoft.AspNet.Identity;
using PagedList;

namespace Booking_Dental_Clinic.Areas.Admin.Controllers
{
    public class ManagerController : Controller
    {
        private DentalClinicEntities db = new DentalClinicEntities();

        // GET: Admin/Manager
        public ActionResult Index(int? page)
        {
            var doctorsRole = db.AspNetRoles.SingleOrDefault(r => r.Name == "NhaSi");

            if (doctorsRole != null)
            {
                var doctors = doctorsRole.AspNetUsers.ToList();
                // Số mục trên mỗi trang
                int pageSize = 5;

                // Số trang hiện tại, mặc định là 1
                int pageNumber = (page ?? 1);
                // Sử dụng gói PagedList.Mvc để phân trang danh sách các bác sĩ
                var pagedDoctors = doctors.ToPagedList(pageNumber, pageSize);

                return View(pagedDoctors);
            }

            return View(new List<AspNetUser>());
        }
        // GET: Admin/Manager/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // GET: Admin/Manager/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Manager/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,FullName")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(aspNetUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspNetUser);
        }

        // GET: Admin/Manager/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: Admin/Manager/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,FullName")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetUser);
        }

        // GET: Admin/Manager/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: Admin/Manager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(aspNetUser);
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

        //public async Task<ActionResult> Approve(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    var user = await db.AspNetUsers.FindAsync(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    user.IsApproved = true;
        //    await db.SaveChangesAsync();

        //    return RedirectToAction("Index");
        //}
        public async Task<ActionResult> Approve(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await db.AspNetUsers.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            user.IsApproved = true;
            await db.SaveChangesAsync();

            // Load the updated user data
            db.Entry(user).Reload();

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> message()
        {
            return View();
        }
    }
}
