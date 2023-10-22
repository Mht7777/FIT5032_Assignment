using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FIT5032_Assignment.Models;
using FIT5032_Assignment.Models.Entites;

namespace FIT5032_Assignment.Controllers
{
    [Authorize(Roles = "Staff")]
    public class ImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Images
        public ActionResult Index()
        {
            return View(db.Images.ToList());
        }

        // GET: Images/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Images images = db.Images.Find(id);
            if (images == null)
            {
                return HttpNotFound();
            }
            return View(images);
        }

        // GET: Images/Create
        public ActionResult Create(int? id)
        {
            ViewBag.AppointmentId = id;
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,AppointmentId")] Images image, HttpPostedFileBase postedFile)
        {
            TempData["SuccessUpolad"] = null;
            TempData["FiledUpolad"] = null;

            // Check if an image already exists for the appointment
            var existingImage = db.Images.FirstOrDefault(i => i.AppointmentId == image.AppointmentId);
            if (existingImage != null)
            {
                // If an existing image is found, construct its server path
                var oldPath = Server.MapPath("~/Uploads/" + existingImage.Path);
                // If the image file exists on the server, delete it
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                // Remove old image record from the database
                db.Images.Remove(existingImage);
                db.SaveChanges();
            }


            ModelState.Clear();
            // Generate a unique file name using a GUID
            var myUniqueFileName = string.Format(@"{0}", Guid.NewGuid());
            // Set the image path to this unique name (the extension will be added later)
            image.Path = myUniqueFileName;
            TryValidateModel(image);
            if (ModelState.IsValid)
            {
                // Determine the server directory where images are stored
                string serverPath = Server.MapPath("~/Uploads/");
                // Get the extension of the uploaded file
                string fileExtension = Path.GetExtension(postedFile.FileName);
                // Combine the unique file name with its extension
                string filePath = image.Path + fileExtension;

                // Update the image model's path to include the extension
                image.Path = filePath;

                // Save the uploaded file to the server directory
                postedFile.SaveAs(serverPath + image.Path);

                // Add the new image record to the database and save changes
                db.Images.Add(image);
                db.SaveChanges();
                TempData["SuccessUpolad"] = "Success Upload!";

                return RedirectToAction("Index","Clinics");
            }
            TempData["FiledUpolad"] = "Failed Upload!";

            return View(image);
        }

        // GET: Images/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Images images = db.Images.Find(id);
            if (images == null)
            {
                return HttpNotFound();
            }
            return View(images);
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Path,Name,AppointmentId")] Images images)
        {
            if (ModelState.IsValid)
            {
                db.Entry(images).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(images);
        }

        // GET: Images/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Images images = db.Images.Find(id);
            if (images == null)
            {
                return HttpNotFound();
            }
            return View(images);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Images images = db.Images.Find(id);
            db.Images.Remove(images);
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
