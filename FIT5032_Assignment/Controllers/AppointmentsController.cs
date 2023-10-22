using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FIT5032_Assignment.Models;
using FIT5032_Assignment.Models.Entites;
using FIT5032_Assignment.Utils;
using Microsoft.AspNet.Identity;

namespace FIT5032_Assignment.Controllers
{
    public class AppointmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Appointments
        [Authorize(Roles = "Patient")]
        public ActionResult Index()
        {
            var appointments = db.Appointments.Include(a => a.Clinic).Include(a => a.Feedback);
            return View(appointments.ToList());
        }
        [Authorize]
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
        [Authorize]
        public ActionResult BookSuccessed()
        {
            return View();
        }

        [Authorize(Roles = "Patient")]
        public ActionResult UserAppointmentDetails(int? id)
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

        [Authorize(Roles = "Patient")]
        public ActionResult UserAppointments()
        {
            //return user owned appointments list
            var userId = User.Identity.GetUserId();
            var userAppointments = db.Appointments.Where(a => a.UserId == userId)
                .Include(a => a.Clinic)
                .Include(a => a.Feedback)
                .Include(a => a.Image);
            return View(userAppointments);
        }


        // GET: Appointments/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.TitleList = GetTitleList();
            ViewBag.GenderList = GetGenderList();
            ViewBag.ClinicId = new SelectList(db.Clinics, "Id", "ClinicName");
            ViewBag.AppointmentId = new SelectList(db.Feedbacks, "AppointmentId", "Comment");
            ViewBag.ScanPartList = new SelectList(GetScanParts());
            ViewBag.AddressList = GetAddress();
            ViewBag.Time = AvailableTime();
            ViewBag.CurrentUserID = User.Identity.GetUserId();
            ViewBag.ClinicAddress = db.Clinics.Find(1).Address;

            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppointmentId,ScanPart,Note,ClinicId,Title,FirstName,LastName,Birthday,PhoneNumber,Email,Gender,IsConfirmed,AppointmentDate,StartTime,EndTime,UserId")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.UserId = User.Identity.GetUserId();
                db.Appointments.Add(appointment);
                db.SaveChanges();
                // Send an email confirmation for the appointment
                SendAppointmentConfirmation(appointment);
                return RedirectToAction("BookSuccessed");
            }
            // Populate the dropdown lists for the view
            ViewBag.ClinicId = new SelectList(db.Clinics, "Id", "ClinicName", appointment.ClinicId);
            ViewBag.AppointmentId = new SelectList(db.Feedbacks, "AppointmentId", "Comment", appointment.AppointmentId);
            return View(appointment);
        }
        // Sends an email confirmation once the appointment is successfully booked.
        private void SendAppointmentConfirmation(Appointment appointment)
        {
            var email = appointment.Email;
            var subject = "Appointment has Booked";
            var content = GenerateAppointmentEmailContent(appointment);
            // Use the EmailSender class to send the email
            EmailSender es = new EmailSender();
            es.Send(email, subject, content);


        }
        // Generates the content for the appointment confirmation email.
        public string GenerateAppointmentEmailContent(Appointment appointment)
        {
            var content = new StringBuilder();
            // Create the content of the email
            content.AppendLine($"Hi {appointment.Title} {appointment.FirstName} {appointment.LastName},<br/>");
            content.AppendLine("<br/>"); // Add a new line for spacing
            content.AppendLine("Thank you for scheduling an appointment with us. Here are the details of your appointment:<br/>");
            content.AppendLine($"Clinic: {db.Clinics.FirstOrDefault(c => c.Id == appointment.ClinicId)?.ClinicName}<br/>");
            content.AppendLine($"Date: {appointment.AppointmentDate.ToShortDateString()}<br/>");
            content.AppendLine($"Time: {appointment.StartTime.ToString("hh:mm tt")} - {appointment.EndTime.ToString("hh:mm tt")}<br/>");
            content.AppendLine($"Scan Part: {appointment.ScanPart}<br/>");
            content.AppendLine($"Note: {appointment.Note}<br/>");
            content.AppendLine("<br/>");
            content.AppendLine("If you have any questions or need to reschedule, please contact us.<br/>");
            content.AppendLine("<br/>");
            content.AppendLine("Thank you,<br/>");
            content.AppendLine("UltraSound");

            return content.ToString();
        }





        // GET: Appointments/Edit/5
        [Authorize(Roles = "Staff")]
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
            ViewBag.TitleList = GetTitleList();
            ViewBag.GenderList = GetGenderList();
            ViewBag.ScanPartList = new SelectList(GetScanParts());
            ViewBag.ClinicId = new SelectList(db.Clinics, "Id", "ClinicName", appointment.ClinicId);
            ViewBag.AppointmentId = new SelectList(db.Feedbacks, "AppointmentId", "Comment", appointment.AppointmentId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppointmentId,ScanPart,Note,ClinicId,Title,FirstName,LastName,Birthday,PhoneNumber,Email,Gender,IsConfirmed,AppointmentDate,StartTime,EndTime,UserId")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Clinics");
            }
            ViewBag.ClinicId = new SelectList(db.Clinics, "Id", "ClinicName", appointment.ClinicId);
            ViewBag.AppointmentId = new SelectList(db.Feedbacks, "AppointmentId", "Comment", appointment.AppointmentId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        [Authorize(Roles = "Staff")]
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
            // Retrieve the appointment with its feedback if has, from the database using the provided ID.
            Appointment appointment = db.Appointments.Find(id);
            FeedbackAndRating feedback = db.Feedbacks.Find(id);
            db.Appointments.Remove(appointment);
            if (feedback != null)
            {
                db.Feedbacks.Remove(feedback);
            }
            // Retrieve the image associated with the appointment, if any.
            Images image = db.Images.FirstOrDefault(i => i.AppointmentId == id);
            if (image != null)
            {
                // Construct the full path to the image on the server.
                string serverPath = Server.MapPath("~/Uploads/");
                string fullPath = serverPath + image.Path;
                if (System.IO.File.Exists(fullPath))
                {
                    try
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    catch (Exception ex)
                    {
                        // Log the exception and handle the error (redirect or show an error view).
                        return View("Error", new { message = "Failed to delete the file from the server. " + ex.Message });
                    }
                }

            }
            db.Images.Remove(image);

            db.SaveChanges();
            return RedirectToAction("Index", "Clinics");
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        // This method returns a predefined list of scan parts.
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
        // This method returns a list of titles for user selection
        public static IEnumerable<SelectListItem> GetTitleList()
        {
            var titleList = new List<SelectListItem>
            {
                new SelectListItem { Text = "Mr.", Value = "Mr." },
                new SelectListItem { Text = "Mrs.", Value = "Mrs." },
                new SelectListItem { Text = "Miss", Value = "Miss" }
            };
            return titleList;
        }

        // This method returns a list of genders for user selection
        public static IEnumerable<SelectListItem> GetGenderList()
        {
            var genderList = new List<SelectListItem>
            {
                new SelectListItem { Text = "Male", Value = "Male" },
                new SelectListItem { Text = "Female", Value = "Female" },
                new SelectListItem { Text = "Other", Value = "Other" }
            };
            return genderList;
        }


        // This method retrieves booked appointment slots for a given date from the database.
        [HttpGet]
        public JsonResult GetBookedSlots(DateTime selectedDate)
        {
            // Hardcoded clinicId in this case.
            var clinicId = 1;
            // Query the database for appointments matching the given clinicId and date.
            var bookedSlots = db.Appointments
                .Where(a => a.ClinicId == clinicId && a.AppointmentDate == selectedDate.Date)
                .Select(a => new { a.StartTime, a.EndTime })
                .ToList();

            return Json(bookedSlots, JsonRequestBehavior.AllowGet);
        }
        // A test method to check if the server receives a selected date.
        public ActionResult CheckTime(string selectedDate)
        {
            if (!string.IsNullOrEmpty(selectedDate))
            {

                return Json(new { success = true, message = "Date received successfully" });
            }
            else
            {

                return Json(new { success = false, message = "Date not received" });
            }
        }
        // This method retrieves the available appointment times for a specific clinic.
        public List<Appointment> AvailableTime()
        {
            // Hardcoded clinicId in this case.
            var clinicid = 1;
            // Query the database for appointment times for the specified clinic.
            var time = db.Clinics.Find(clinicid).Appointments.Select(a =>
                new Appointment
                {
                    AppointmentDate = a.AppointmentDate,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime
                }).ToList();
            return time;
        }



        // This method retrieves unavailable appointment times for a given date.
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


        //get addrss list from database
        public List<string> GetAddress()
        {
            var addresses = db.Clinics.Select(c => c.Address).ToList();
            return addresses;
        }
        // This method returns the list of addresses in a JSON format.
        public JsonResult GetAddresses()
        {
            var addresses = GetAddress();
            return Json(addresses, JsonRequestBehavior.AllowGet);
        }

    }
}
