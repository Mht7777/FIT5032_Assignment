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

        //get addrss list from database
        public List<string> GetAddress()
        {
            var addresses = db.Clinics.Select(c => c.Address).ToList();
            return addresses;
        }

        public JsonResult GetAddresses()
        {
            var addresses = GetAddress();
            return Json(addresses, JsonRequestBehavior.AllowGet);
        }


        // GET: Appointments/Create
        public ActionResult Create()
        {
            ViewBag.ClinicId = new SelectList(db.Clinics, "Id", "ClinicName");
            ViewBag.AppointmentId = new SelectList(db.Feedbacks, "AppointmentId", "Comment");
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "UserId");
            ViewBag.ScanPartList = new SelectList(GetScanParts());
            ViewBag.AddressList = GetAddress();
            ViewBag.Time = AvailableTime();
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppointmentId,PatientId,ScanPart,Note,ClinicId,Title,IsConfirmed,AppointmentDateTime,UserId")] Appointment appointment)
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
        public ActionResult Edit([Bind(Include = "AppointmentId,PatientId,ScanPart,Note,ClinicId,Title,IsConfirmed,AppointmentDateTime,UserId")] Appointment appointment)
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

        private IEnumerable<string> GetScanParts()
        {
            var scanParts = new List<string>
            {
                "Abdomen", "Pelvic", "Obstetric", "Transvaginal", "Cardiac",
                "Renal", "Thyroid", "Breast", "Musculoskeletal", "Carotid",
                "Doppler", "Prostate", "Testicular/Scrotal", "Soft tissue",
                "Vascular", "Fetal echo", "Lung", "Intestinal", "Liver",
                "Gallbladder", "Pancreas", "Spleen", "Transesophageal echocardiogram"
            };
            return scanParts;

        }

        [HttpGet]
        public JsonResult GetBookedSlots(DateTime selectedDate)
        {
            var clinicId = 1;
            var bookedSlots = db.Appointments
                .Where(a => a.ClinicId == clinicId && a.AppointmentDate == selectedDate.Date)
                .Select(a => new { a.StartTime, a.EndTime })
                .ToList();

            return Json(bookedSlots, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckTime(string selectedDate)
        {
            if (!string.IsNullOrEmpty(selectedDate))
            {
                // The selected date has been passed to the controller
                // You can use it as needed
                // Optionally, you can return a response
                return Json(new { success = true, message = "Date received successfully" });
            }
            else
            {
                // The selected date was not received
                // Handle the case where no date is provided
                return Json(new { success = false, message = "Date not received" });
            }
        }

        public List<Appointment> AvailableTime()
        {
            var clinicid = 1;
            var time = db.Clinics.Find(clinicid).Appointments.Select(a =>
                new Appointment
                {
                    AppointmentDate = a.AppointmentDate,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime
                }).ToList();
            return time;
        }




        [HttpGet]
        public JsonResult GetAppointmentTimes(string date)
        {
            DateTime selectedDate;
            if (!DateTime.TryParse(date, out selectedDate))
            {
                return Json(new { error = "Invalid date." }, JsonRequestBehavior.AllowGet);
            }

            var unavailableTimes = db.Appointments
                .Where(a => DbFunctions.TruncateTime(a.AppointmentDate) == selectedDate.Date)
                .ToList()  // Pull data into memory
                .Select(a => new
                {
                    StartTime = a.StartTime.ToString("HH:mm"),
                    EndTime = a.EndTime.ToString("HH:mm")
                });

            return Json(unavailableTimes, JsonRequestBehavior.AllowGet);
        }





    }
}
