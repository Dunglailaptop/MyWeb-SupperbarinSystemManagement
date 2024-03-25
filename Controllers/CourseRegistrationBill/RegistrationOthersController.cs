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
    public class RegistrationOthersController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: RegistrationOthers
        public ActionResult Index()
        {
            var registrationOthers = db.RegistrationOthers.Include(r => r.Registration).Include(r => r.RevenueReference);
            return View(registrationOthers.ToList());
        }

        // GET: RegistrationOthers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationOther registrationOther = db.RegistrationOthers.Find(id);
            if (registrationOther == null)
            {
                return HttpNotFound();
            }
            return View(registrationOther);
        }

        // GET: RegistrationOthers/Create
        public ActionResult Create()
        {
            ViewBag.IdRegistration = new SelectList(db.Registrations, "Id", "Description");
            ViewBag.IdReference = new SelectList(db.RevenueReferences, "Id", "Code");
            return View();
        }

        // POST: RegistrationOthers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdRegistration,IdReference,Price,Discount,TotalAmount,Status,Amount")] RegistrationOther registrationOther)
        {
            if (ModelState.IsValid)
            {
                db.RegistrationOthers.Add(registrationOther);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdRegistration = new SelectList(db.Registrations, "Id", "Description", registrationOther.IdRegistration);
            ViewBag.IdReference = new SelectList(db.RevenueReferences, "Id", "Code", registrationOther.IdReference);
            return View(registrationOther);
        }

        // GET: RegistrationOthers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationOther registrationOther = db.RegistrationOthers.Find(id);
            if (registrationOther == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdRegistration = new SelectList(db.Registrations, "Id", "Description", registrationOther.IdRegistration);
            ViewBag.IdReference = new SelectList(db.RevenueReferences, "Id", "Code", registrationOther.IdReference);
            return View(registrationOther);
        }

        // POST: RegistrationOthers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdRegistration,IdReference,Price,Discount,TotalAmount,Status,Amount")] RegistrationOther registrationOther)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registrationOther).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdRegistration = new SelectList(db.Registrations, "Id", "Description", registrationOther.IdRegistration);
            ViewBag.IdReference = new SelectList(db.RevenueReferences, "Id", "Code", registrationOther.IdReference);
            return View(registrationOther);
        }

        // GET: RegistrationOthers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationOther registrationOther = db.RegistrationOthers.Find(id);
            if (registrationOther == null)
            {
                return HttpNotFound();
            }
            return View(registrationOther);
        }

        // POST: RegistrationOthers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RegistrationOther registrationOther = db.RegistrationOthers.Find(id);
            db.RegistrationOthers.Remove(registrationOther);
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
