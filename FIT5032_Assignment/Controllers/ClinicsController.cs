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
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace FIT5032_Assignment.Controllers
{
    [Authorize(Roles = "Staff")]
    public class ClinicsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // The Index action is responsible for loading the main page/view of the application.
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            // Getting the staff record for the logged-in user.
            var staff = db.Staffs.FirstOrDefault(s => s.UserId == userId);

            if (staff != null)
            {
                var clinic = db.Clinics.Find(staff.ClinicId);
                // If the clinic was found
                if (clinic != null)
                {
                    // Retrieve appointment data for the current month, week, and day.
                    // The data is fetched in a format suitable for charting/graphing.
                    List<DataPoint> monthdataPoints = FetchCurrentMonthAppointment(clinic);
                    ViewBag.MonthDataPoints = JsonConvert.SerializeObject(monthdataPoints);

                    List<DataPoint> weekdataPoints = FetchCurrentWeekAppointment(clinic);
                    ViewBag.WeekDataPoints = JsonConvert.SerializeObject(weekdataPoints);

                    List<DataPoint> todaydataPoints = FetchTodayAppointment(clinic);
                    ViewBag.TodayDataPoints = JsonConvert.SerializeObject(todaydataPoints);


                    return View(clinic);
                }
                // If the clinic was not found, redirect to an error page.
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
        // This method fetches and processes appointment data for the current month.
        private List<DataPoint> FetchCurrentMonthAppointment(Clinic clinic)
        {
            // Define the start and end dates of the current month.
            DateTime startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            // Fetch all appointments for the specified clinic that fall within the current month.
            var currentMonthAppointments = db.Appointments.Where(a => a.ClinicId == clinic.Id &&
                                          a.AppointmentDate >= startOfMonth &&
                                          a.AppointmentDate <= endOfMonth).ToList();

            // Group by AppointmentDate and count the number of appointments for each day
            var groupedAppointments = currentMonthAppointments
                .GroupBy(a => a.AppointmentDate.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToList();

            // Convert the grouped and counted appointment data into a format suitable for charting.
            List<DataPoint> dataPoints = groupedAppointments
                .Select(g => new DataPoint((double)g.Date.Day, g.Count))
                .ToList();

            return dataPoints;
        }

        private List<DataPoint> FetchTodayAppointment(Clinic clinic)
        {
            DateTime today = DateTime.Now.Date;
            // Fetch today's appointments for the given clinic.

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
            // Determine the current date and the start of the week (Monday).
            DateTime today = DateTime.Now.Date;
            int daysFromMonday = (int)today.DayOfWeek - (int)DayOfWeek.Monday;
            DateTime startOfWeek = today.AddDays(-daysFromMonday);

            // The end of the week would be the start of the week plus 6 days.
            DateTime endOfWeek = startOfWeek.AddDays(6);

            // Fetch appointments for the given clinic that fall within the current week.
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

        // Exports current month's appointment data to a CSV file.
        public ActionResult ExportAppointmentsToCSV()
        {
            // Reset export messages
            TempData["SuccessExport"] = null;
            TempData["FiledExport"] = null;

            // Retrieve current user's ID
            var userId = User.Identity.GetUserId();

            // Attempt to retrieve the associated staff record for the logged-in user
            var staff = db.Staffs.FirstOrDefault(s => s.UserId == userId);
            if (staff != null)
            {
                // If staff exists, fetch associated clinic
                var clinic = db.Clinics.Find(staff.ClinicId);
                if (clinic != null)
                {
                    // Fetch current month's appointment data for the clinic
                    var dataPoints = FetchCurrentMonthAppointment(clinic);

                    var csvData = new StringBuilder();

                    // Determine current month and year for CSV header
                    string currentYear = DateTime.Now.ToString("yyyy");
                    string currentMonth = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);

                    // Append CSV headers and data
                    csvData.AppendLine($"{currentMonth}.{currentYear}");
                    csvData.AppendLine("Date,Count");
                    foreach (var point in dataPoints)
                    {
                        csvData.AppendLine($"{point.X},{point.Y}");
                    }

                    // Define where to save the CSV file
                    var folderPath = Server.MapPath("~/ExportData/");

                    DateTime now = DateTime.Now;
                    string timestamp = now.ToString("yyyyMMdd_HHmmss");
                    string filename = $"{timestamp}.csv";
                    var filePath = Path.Combine(folderPath, filename);
                    System.IO.File.WriteAllText(filePath, csvData.ToString());

                    TempData["SuccessExport"] = "Export CSV Successed!";
                    return RedirectToAction("Index");
                }
            }
            TempData["FiledExport"] = "Export CSV Failed!";
            return View();
        }

        public ActionResult ExportAppointmentsToPDF()
        {
            // Reset export messages
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
                    // Fetch current month's appointment data for the clinic
                    var dataPoints = FetchCurrentMonthAppointment(clinic);
                    var pdfData = new StringBuilder();
                    // Determine current month and year for table header
                    string currentYear = DateTime.Now.ToString("yyyy");
                    string currentMonth = DateTime.Now.ToString("MM");

                    // Start building the HTML representation for the PDF
                    pdfData.Append("<table style='border: 1px solid black;'>");
                    pdfData.Append("<tr>");
                    pdfData.Append($"<th style='border: 1px solid black'>{currentYear}.{currentMonth}</th>");
                    pdfData.Append("</tr>");

                    pdfData.Append("<tr>");
                    pdfData.Append("<th style='border: 1px solid black'>Date</th>");
                    pdfData.Append("<th style='border: 1px solid black'>Number of Appointment</th>");
                    pdfData.Append("</tr>");

                    // Building the Data rows.
                    for (int i = 0; i < dataPoints.Count; i++)
                    {
                        var dataPoint = dataPoints[i];
                        pdfData.Append("<tr>");
                        pdfData.Append($"<td style='border: 1px solid black'>{dataPoint.X}</td>"); // Date
                        pdfData.Append($"<td style='border: 1px solid black'>{dataPoint.Y}</td>"); // Count
                        pdfData.Append("</tr>");
                    }
                    pdfData.Append("</table>");

                    // Convert HTML representation to PDF
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
        // Exports current month's appointment data to an Excel file.
        public ActionResult ExportAppointmentsToExcel()
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
                    // Fetch current month's appointment data for the clinic
                    var dataPoints = FetchCurrentMonthAppointment(clinic);

                    // Determine current month and year
                    string currentYear = DateTime.Now.ToString("yyyy");
                    string currentMonth = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);

                    DataTable Dt = new DataTable();
                    Dt.Columns.Add("Date", typeof(string));
                    Dt.Columns.Add("Count", typeof(string));
                    // Populate the DataTable from the data points
                    foreach (var data in dataPoints)
                    {
                        DataRow row = Dt.NewRow();
                        row[0] = data.X.ToString();
                        row[1] = data.Y.ToString();
                        Dt.Rows.Add(row);
                    }
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    // Generate the Excel file
                    using (var package = new ExcelPackage())
                    {
                        var ws = package.Workbook.Worksheets.Add("Appointments");
                        ws.Cells["A1"].LoadFromDataTable(Dt, true);

                        var folderPath = Server.MapPath("~/ExportData/");


                        // Define where to save the Excel file
                        DateTime now = DateTime.Now;
                        string timestamp = now.ToString("yyyyMMdd_HHmmss");
                        string filename = $"{timestamp}.xlsx";
                        var filePath = Path.Combine(folderPath, filename);

                        package.SaveAs(new FileInfo(filePath));
                    }

                    TempData["SuccessExport"] = "Export Excel Succeeded!";
                    return RedirectToAction("Index");
                }
            }

            TempData["FailedExport"] = "Export Excel Failed!";
            return View();
        }







        [HttpPost]
        public ActionResult SendEmail(EmailViewModel model)
        {
            // Reset TempData messages
            TempData["SuccessMessage"] = null;
            TempData["ErrorMessage"] = null;



            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the attachment file from the model
                    HttpPostedFileBase file = model.Attachment;
                    // Create a new instance of the EmailSender class
                    EmailSender es = new EmailSender();
                    // Split the provided email recipients by semicolon and clean up any extra whitespace
                    var recipientEmails = model.Emails.Split(';').Select(email => email.Trim()).ToList();
                    // Send the email using the EmailSender instance
                    es.Send(recipientEmails, model.Subject, model.Contents, file);
                    // If successful, show success message and clear the model state
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
