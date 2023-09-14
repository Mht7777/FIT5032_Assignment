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
    public class FeedbackAndRatingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FeedbackAndRatings
        public ActionResult Index()
        {
            var feedbacks = db.Feedbacks.Include(f => f.Appointment);
            return View(feedbacks.ToList());
        }

        // GET: FeedbackAndRatings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedbackAndRating feedbackAndRating = db.Feedbacks.Find(id);
            if (feedbackAndRating == null)
            {
                return HttpNotFound();
            }
            return View(feedbackAndRating);
        }

        // GET: FeedbackAndRatings/Create
        public ActionResult Create()
        {
            ViewBag.AppointmentId = new SelectList(db.Appointments, "AppointmentId", "PatientName");
            return View();
        }

        // POST: FeedbackAndRatings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppointmentId,Rating,Comment")] FeedbackAndRating feedbackAndRating)
        {
            if (ModelState.IsValid)
            {
                db.Feedbacks.Add(feedbackAndRating);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AppointmentId = new SelectList(db.Appointments, "AppointmentId", "PatientName", feedbackAndRating.AppointmentId);
            return View(feedbackAndRating);
        }

        // GET: FeedbackAndRatings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedbackAndRating feedbackAndRating = db.Feedbacks.Find(id);
            if (feedbackAndRating == null)
            {
                return HttpNotFound();
            }
            ViewBag.AppointmentId = new SelectList(db.Appointments, "AppointmentId", "PatientName", feedbackAndRating.AppointmentId);
            return View(feedbackAndRating);
        }

        // POST: FeedbackAndRatings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppointmentId,Rating,Comment")] FeedbackAndRating feedbackAndRating)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feedbackAndRating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AppointmentId = new SelectList(db.Appointments, "AppointmentId", "PatientName", feedbackAndRating.AppointmentId);
            return View(feedbackAndRating);
        }

        // GET: FeedbackAndRatings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedbackAndRating feedbackAndRating = db.Feedbacks.Find(id);
            if (feedbackAndRating == null)
            {
                return HttpNotFound();
            }
            return View(feedbackAndRating);
        }

        // POST: FeedbackAndRatings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FeedbackAndRating feedbackAndRating = db.Feedbacks.Find(id);
            db.Feedbacks.Remove(feedbackAndRating);
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
