using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FIT5032_Assignment.Models;
using FIT5032_Assignment.Models.Entites;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FIT5032_Assignment.Controllers
{
    // This controller is accessible only to users with the "Admin" role.
    [Authorize(Roles = "Admin")]
    public class AdminsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admins
        public ActionResult Index()
        {
            var users = db.Users.ToList();

            return View(users);
        }

        // GET: Admins/Details/5
        public  ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdminId,UserId")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Admins/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,Email")] IdentityUser user)
        {
            if (ModelState.IsValid)
            {
                var storedUser = db.Users.Find(user.Id);
                if (storedUser == null)
                {
                    return HttpNotFound();
                }

                storedUser.UserName = user.UserName;
                storedUser.Email = user.Email;

                db.Entry(storedUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }


        // GET: Admins/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }


            DeleteUser(id);
            

            db.Users.Remove(user);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        private void DeleteUser(string userId)
        {
            var appointments = db.Appointments.Where(a => a.UserId == userId).ToList();
            foreach (var appointment in appointments)
            {
                var feedback = db.Feedbacks.Find(appointment.AppointmentId);
                if (feedback != null)
                {
                    db.Feedbacks.Remove(feedback);
                }

                var image = db.Images.FirstOrDefault(i => i.AppointmentId == appointment.AppointmentId);
                if (image != null)
                {
                    string serverPath = Server.MapPath("~/Uploads/");
                    string fullPath = serverPath + image.Path;
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    db.Images.Remove(image);
                }

                db.Appointments.Remove(appointment);
                var staff = db.Staffs.Find(userId);
                if (staff != null) { db.Staffs.Remove(staff); }
            }
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
