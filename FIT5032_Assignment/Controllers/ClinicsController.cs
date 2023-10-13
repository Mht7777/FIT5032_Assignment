using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FIT5032_Assignment.Models;
using FIT5032_Assignment.Models.Entites;
using FIT5032_Assignment.Utils;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace FIT5032_Assignment.Controllers
{
    [Authorize(Roles = "Staff")]
    public class ClinicsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            // Getting the staff record for the logged-in user.
            var staff = db.Staffs.FirstOrDefault(s => s.UserId == userId);

            if (staff != null)
            {
                var clinic = db.Clinics.Find(staff.ClinicId);

                if (clinic != null)
                {
                    List<DataPoint> dataPoints = FetchCurrentMonthAppointment(clinic);
                    ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

                    return View(clinic);
                }
                else
                {
                    return RedirectToAction("ErrorClinicNotFound");
                }
            }
            else
            {
                return RedirectToAction("ErrorStaffNotFound");
            }
        }
        private List<DataPoint> FetchCurrentMonthAppointment(Clinic clinic)
        {
            DateTime startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var currentMonthAppointments = db.Appointments.Where(a => a.ClinicId == clinic.Id &&
                                          a.AppointmentDate >= startOfMonth &&
                                          a.AppointmentDate <= endOfMonth).ToList();

            // Group by AppointmentDate and count the number of appointments for each day
            var groupedAppointments = currentMonthAppointments
                .GroupBy(a => a.AppointmentDate.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToList();

            // Convert the grouped data into a format suitable for the chart
            List<DataPoint> dataPoints = groupedAppointments
                .Select(g => new DataPoint((double)g.Date.Day, g.Count))
                .ToList();

            return dataPoints;
        }


        [HttpPost]
        public ActionResult SendEmail(EmailViewModel model)
        {
            TempData["SuccessMessage"] = null;
            TempData["ErrorMessage"] = null;



            if (ModelState.IsValid)
            {
                try
                {
                    HttpPostedFileBase file = model.Attachment;
                    EmailSender es = new EmailSender();
                    var recipientEmails = model.Emails.Split(';').Select(email => email.Trim()).ToList();

                    es.Send(recipientEmails, model.Subject, model.Contents, file); 
                    TempData["SuccessMessage"] = "Email has been sent.";
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)

                {
                    TempData["ErrorMessage"] = "Email has not been sent. Error: " + ex.Message;
                    return RedirectToAction("Index");
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
            TempData["ErrorMessage"] += "Email has not been sent.";
            return RedirectToAction("Index");
        }

        public ActionResult PatientAppointmentDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);

            if (appointment == null)
            {
                return HttpNotFound();
            }
            var image = db.Images.FirstOrDefault(i => i.AppointmentId == id);
            if (image != null)
            {
                ViewBag.ImagePath = image.Path;
                ViewBag.ImageName = image.Name;
            }

            return View(appointment);
        }



        // GET: Clinics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinic clinic = db.Clinics.Find(id);
            if (clinic == null)
            {
                return HttpNotFound();
            }
            return View(clinic);
        }

        // GET: Clinics/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clinics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ClinicName,Address")] Clinic clinic)
        {
            if (ModelState.IsValid)
            {
                db.Clinics.Add(clinic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(clinic);
        }

        // GET: Clinics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinic clinic = db.Clinics.Find(id);
            if (clinic == null)
            {
                return HttpNotFound();
            }
            return View(clinic);
        }

        // POST: Clinics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ClinicName,Address")] Clinic clinic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clinic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clinic);
        }

        // GET: Clinics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinic clinic = db.Clinics.Find(id);
            if (clinic == null)
            {
                return HttpNotFound();
            }
            return View(clinic);
        }

        // POST: Clinics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clinic clinic = db.Clinics.Find(id);
            db.Clinics.Remove(clinic);
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
