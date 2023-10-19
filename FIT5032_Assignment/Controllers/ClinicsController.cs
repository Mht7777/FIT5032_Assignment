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
using FIT5032_Assignment.Models;
using FIT5032_Assignment.Models.Entites;
using FIT5032_Assignment.Utils;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.tool.xml;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System.Globalization;


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
                    List<DataPoint> monthdataPoints = FetchCurrentMonthAppointment(clinic);
                    ViewBag.MonthDataPoints = JsonConvert.SerializeObject(monthdataPoints);

                    List<DataPoint> weekdataPoints = FetchCurrentWeekAppointment(clinic);
                    ViewBag.WeekDataPoints = JsonConvert.SerializeObject(weekdataPoints);

                    List<DataPoint> todaydataPoints = FetchTodayAppointment(clinic);
                    ViewBag.TodayDataPoints = JsonConvert.SerializeObject(todaydataPoints);


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

        private List<DataPoint> FetchTodayAppointment(Clinic clinic)
        {
            DateTime today = DateTime.Now.Date;

            var todayAppointments = db.Appointments.Where(a => a.ClinicId == clinic.Id &&
                                              a.AppointmentDate == today).ToList();

            // Group by AppointmentDate and count the number of appointments for each day
            var groupedAppointments = todayAppointments
                .GroupBy(a => a.AppointmentDate.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToList();

            // Convert the grouped data into a format suitable for the chart
            List<DataPoint> dataPoints = groupedAppointments
                .Select(g => new DataPoint((double)g.Date.Day, g.Count))
                .ToList();

            return dataPoints;
        }

        private List<DataPoint> FetchCurrentWeekAppointment(Clinic clinic)
        {

            DateTime today = DateTime.Now.Date;
            int daysFromMonday = (int)today.DayOfWeek - (int)DayOfWeek.Monday;
            DateTime startOfWeek = today.AddDays(-daysFromMonday);

            // The end of the week would be the start of the week plus 6 days (since we count the start day).
            DateTime endOfWeek = startOfWeek.AddDays(6);


            var currentWeekAppointments = db.Appointments.Where(a => a.ClinicId == clinic.Id &&
                                          a.AppointmentDate >= startOfWeek &&
                                          a.AppointmentDate <= endOfWeek).ToList();

            // Group by AppointmentDate and count the number of appointments for each day
            var groupedAppointments = currentWeekAppointments
                .GroupBy(a => a.AppointmentDate.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToList();

            // Convert the grouped data into a format suitable for the chart
            List<DataPoint> dataPoints = groupedAppointments
                .Select(g => new DataPoint((double)g.Date.Day, g.Count))
                .ToList();

            return dataPoints;
        }


        public ActionResult ExportAppointmentsToCSV()
        {
            TempData["SuccessExport"] = null;
            TempData["FiledExport"] = null;

            var userId = User.Identity.GetUserId();

            // Getting the staff record for the logged-in user.
            var staff = db.Staffs.FirstOrDefault(s => s.UserId == userId);
            if (staff != null)
            {
                var clinic = db.Clinics.Find(staff.ClinicId);
                if (clinic != null)
                {
                    var dataPoints = FetchCurrentMonthAppointment(clinic);

                    var csvData = new StringBuilder();

                    string currentYear = DateTime.Now.ToString("yyyy");
                    string currentMonth = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);

                    csvData.AppendLine($"{currentMonth}.{currentYear}");
                    csvData.AppendLine("Date,Count"); // CSV header
                        
                    foreach (var point in dataPoints)
                    {
                        csvData.AppendLine($"{point.X},{point.Y}");
                    }

                    // Save CSV to a folder
                    var folderPath = Server.MapPath("~/ExportData/");

                    // Ensure directory exists
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    DateTime now = DateTime.Now;
                    string timestamp = now.ToString("yyyyMMdd_HHmmss");
                    string filename = $"{timestamp}.csv";
                    var filePath = Path.Combine(folderPath, filename);
                    System.IO.File.WriteAllText(filePath, csvData.ToString());
                    TempData["SuccessExport"] = "Export CSV Successed!";
                    // Return a success message or redirect to another action.
                    return RedirectToAction("Index");
                }
            }
            TempData["FiledExport"] = "Export CSV Failed!";

            return View();
        }

        public ActionResult ExportAppointmentsToPDF()
        {
            TempData["SuccessExport"] = null;
            TempData["FiledExport"] = null;
            var userId = User.Identity.GetUserId();

            // Getting the staff record for the logged-in user.
            var staff = db.Staffs.FirstOrDefault(s => s.UserId == userId);
            if (staff != null)
            {
                var clinic = db.Clinics.Find(staff.ClinicId);
                if (clinic != null)
                {
                    var dataPoints = FetchCurrentMonthAppointment(clinic);
                    var pdfData = new StringBuilder();

                    string currentYear = DateTime.Now.ToString("yyyy");
                    string currentMonth = DateTime.Now.ToString("MM");


                    pdfData.Append("<table border='1' cellpadding='5' cellspacing='0' style='border: 1px solid #ccc;font-family: Arial; font-size: 10pt;'>");
                    pdfData.Append("<tr>");
                    pdfData.Append($"<th style='background-color: #B8DBFD;border: 1px solid #ccc'>{currentYear}.{currentMonth}</th>");
                    pdfData.Append("</tr>");

                    pdfData.Append("<tr>");
                    pdfData.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Date</th>");
                    pdfData.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Number of Appointment</th>");
                    pdfData.Append("</tr>");

                    // Building the Data rows.
                    for (int i = 0; i < dataPoints.Count; i++)
                    {
                        var dataPoint = dataPoints[i];
                        pdfData.Append("<tr>");
                        pdfData.Append($"<td style='border: 1px solid #ccc'>{dataPoint.X}</td>"); // Date
                        pdfData.Append($"<td style='border: 1px solid #ccc'>{dataPoint.Y}</td>"); // Count
                        pdfData.Append("</tr>");
                    }
                    pdfData.Append("</table>");

                    using (MemoryStream stream = new MemoryStream())
                    {
                        StringReader sr = new StringReader(pdfData.ToString());
                        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();
                        pdfDoc.NewPage();
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                        pdfDoc.Close();

                        // Save to a folder
                        var folderPath = Server.MapPath("~/ExportData/");

                        // Ensure directory exists
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        DateTime now = DateTime.Now;
                        string timestamp = now.ToString("yyyyMMdd_HHmmss");
                        string filename = $"{timestamp}.pdf";

                        var filePath = Path.Combine(folderPath, filename);
                        System.IO.File.WriteAllBytes(filePath, stream.ToArray());
                        TempData["SuccessExport"] = "Export PDF Successed!";
                        // Return a success message or redirect to another action.
                        return RedirectToAction("Index", new { message = "Data exported successfully!" });
                    }
                }
            }
            TempData["FiledExport"] = "Export PDF Failed!";
            return View();
        }

        public ActionResult ExportAppointmentsToTXT()
        {
            TempData["SuccessExport"] = null;
            TempData["FailedExport"] = null;

            var userId = User.Identity.GetUserId();

            // Getting the staff record for the logged-in user.
            var staff = db.Staffs.FirstOrDefault(s => s.UserId == userId);
            if (staff != null)
            {
                var clinic = db.Clinics.Find(staff.ClinicId);
                if (clinic != null)
                {
                    var dataPoints = FetchCurrentMonthAppointment(clinic);

                    var txtData = new StringBuilder();

                    string currentYear = DateTime.Now.ToString("yyyy");
                    string currentMonth = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);

                    txtData.AppendLine($"{currentMonth}.{currentYear}");
                    txtData.AppendLine("Date\tCount"); // TXT header with tab-separated values

                    foreach (var point in dataPoints)
                    {
                        txtData.AppendLine($"{point.X}\t{point.Y}");
                    }

                    // Save TXT to a folder
                    var folderPath = Server.MapPath("~/ExportData/");

                    // Ensure directory exists
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    DateTime now = DateTime.Now;
                    string timestamp = now.ToString("yyyyMMdd_HHmmss");
                    string filename = $"{timestamp}.txt";
                    var filePath = Path.Combine(folderPath, filename);
                    System.IO.File.WriteAllText(filePath, txtData.ToString());
                    TempData["SuccessExport"] = "Export TXT Succeeded!";
                    // Return a success message or redirect to another action.
                    return RedirectToAction("Index");
                }
            }
            TempData["FailedExport"] = "Export TXT Failed!";
            return View();
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
