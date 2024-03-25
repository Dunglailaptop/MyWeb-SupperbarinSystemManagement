using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SuperbrainManagement.Models;

namespace SuperbrainManagement.Controllers.CourseRegistrationBill
{
    public class RegistrationCoursesController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: RegistrationCourses
        public ActionResult Index()
        {
            var registrationCourses = db.RegistrationCourses.Include(r => r.Course).Include(r => r.Registration);
            return View(registrationCourses.ToList());
        }

        // GET: RegistrationCourses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationCourse registrationCourse = db.RegistrationCourses.Find(id);
            if (registrationCourse == null)
            {
                return HttpNotFound();
            }
            return View(registrationCourse);
        }

        // GET: RegistrationCourses/Create
        public ActionResult Create()
        {
            ViewBag.IdCourse = new SelectList(db.Courses, "Id", "Code");
            ViewBag.IdRegistration = new SelectList(db.Registrations, "Id", "Description");
            return View();
        }

        // POST: RegistrationCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdRegistration,IdCourse,Price,Discount,TotalAmount,Status,Amount,Enable,StatusExchangeCourse,DateExchangeCourse,StatusExtend,DateExtend,StatusReserve,DateReserve,Description,StatusJoinClass,DateJoinClass")] RegistrationCourse registrationCourse)
        {
            if (ModelState.IsValid)
            {
                db.RegistrationCourses.Add(registrationCourse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCourse = new SelectList(db.Courses, "Id", "Code", registrationCourse.IdCourse);
            ViewBag.IdRegistration = new SelectList(db.Registrations, "Id", "Description", registrationCourse.IdRegistration);
            return View(registrationCourse);
        }

        // GET: RegistrationCourses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationCourse registrationCourse = db.RegistrationCourses.Find(id);
            if (registrationCourse == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCourse = new SelectList(db.Courses, "Id", "Code", registrationCourse.IdCourse);
            ViewBag.IdRegistration = new SelectList(db.Registrations, "Id", "Description", registrationCourse.IdRegistration);
            return View(registrationCourse);
        }

        // POST: RegistrationCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdRegistration,IdCourse,Price,Discount,TotalAmount,Status,Amount,Enable,StatusExchangeCourse,DateExchangeCourse,StatusExtend,DateExtend,StatusReserve,DateReserve,Description,StatusJoinClass,DateJoinClass")] RegistrationCourse registrationCourse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registrationCourse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCourse = new SelectList(db.Courses, "Id", "Code", registrationCourse.IdCourse);
            ViewBag.IdRegistration = new SelectList(db.Registrations, "Id", "Description", registrationCourse.IdRegistration);
            return View(registrationCourse);
        }

        // GET: RegistrationCourses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationCourse registrationCourse = db.RegistrationCourses.Find(id);
            if (registrationCourse == null)
            {
                return HttpNotFound();
            }
            return View(registrationCourse);
        }

        // POST: RegistrationCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RegistrationCourse registrationCourse = db.RegistrationCourses.Find(id);
            db.RegistrationCourses.Remove(registrationCourse);
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
