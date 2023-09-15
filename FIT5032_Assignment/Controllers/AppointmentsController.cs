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

namespace FIT5032_Assignment.Controllers
{
    public class AppointmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Appointments
        public ActionResult Index()
        {
            var appointments = db.Appointments.Include(a => a.Clinic).Include(a => a.Feedback).Include(a => a.Patient);
            return View(appointments.ToList());
        }

        // GET: Appointments/Details/5
        public ActionResult Details(int? id)
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
            return View(appointment);
        }

        // GET: Appointments/Create
        public ActionResult Create()
        {
            ViewBag.ClinicId = new SelectList(db.Clinics, "Id", "ClinicName");
            ViewBag.AppointmentId = new SelectList(db.Feedbacks, "AppointmentId", "Comment");
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "UserId");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppointmentId,PatientId,ScanPart,Note,ClinicId,Title,IsConfirmed,UserId")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClinicId = new SelectList(db.Clinics, "Id", "ClinicName", appointment.ClinicId);
            ViewBag.AppointmentId = new SelectList(db.Feedbacks, "AppointmentId", "Comment", appointment.AppointmentId);
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "UserId", appointment.PatientId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.ClinicId = new SelectList(db.Clinics, "Id", "ClinicName", appointment.ClinicId);
            ViewBag.AppointmentId = new SelectList(db.Feedbacks, "AppointmentId", "Comment", appointment.AppointmentId);
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "UserId", appointment.PatientId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppointmentId,PatientId,ScanPart,Note,ClinicId,Title,IsConfirmed,UserId")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClinicId = new SelectList(db.Clinics, "Id", "ClinicName", appointment.ClinicId);
            ViewBag.AppointmentId = new SelectList(db.Feedbacks, "AppointmentId", "Comment", appointment.AppointmentId);
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "UserId", appointment.PatientId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public ActionResult Delete(int? id)
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
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
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
