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
    public class RegistrationProductsController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: RegistrationProducts
        public ActionResult Index()
        {
            var registrationProducts = db.RegistrationProducts.Include(r => r.Product).Include(r => r.Promotion).Include(r => r.Registration);
            return View(registrationProducts.ToList());
        }

        // GET: RegistrationProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationProduct registrationProduct = db.RegistrationProducts.Find(id);
            if (registrationProduct == null)
            {
                return HttpNotFound();
            }
            return View(registrationProduct);
        }

        // GET: RegistrationProducts/Create
        public ActionResult Create()
        {
            ViewBag.IdProduct = new SelectList(db.Products, "Id", "Name");
            ViewBag.IdPromotion = new SelectList(db.Promotions, "Id", "Code");
            ViewBag.IdRegistration = new SelectList(db.Registrations, "Id", "Description");
            return View();
        }

        // POST: RegistrationProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdRegistration,IdProduct,Price,Discount,TotalAmount,Status,Amount,IsGift,IdPromotion")] RegistrationProduct registrationProduct)
        {
            if (ModelState.IsValid)
            {
                db.RegistrationProducts.Add(registrationProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdProduct = new SelectList(db.Products, "Id", "Name", registrationProduct.IdProduct);
            ViewBag.IdPromotion = new SelectList(db.Promotions, "Id", "Code", registrationProduct.IdPromotion);
            ViewBag.IdRegistration = new SelectList(db.Registrations, "Id", "Description", registrationProduct.IdRegistration);
            return View(registrationProduct);
        }

        // GET: RegistrationProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationProduct registrationProduct = db.RegistrationProducts.Find(id);
            if (registrationProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdProduct = new SelectList(db.Products, "Id", "Name", registrationProduct.IdProduct);
            ViewBag.IdPromotion = new SelectList(db.Promotions, "Id", "Code", registrationProduct.IdPromotion);
            ViewBag.IdRegistration = new SelectList(db.Registrations, "Id", "Description", registrationProduct.IdRegistration);
            return View(registrationProduct);
        }

        // POST: RegistrationProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdRegistration,IdProduct,Price,Discount,TotalAmount,Status,Amount,IsGift,IdPromotion")] RegistrationProduct registrationProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registrationProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdProduct = new SelectList(db.Products, "Id", "Name", registrationProduct.IdProduct);
            ViewBag.IdPromotion = new SelectList(db.Promotions, "Id", "Code", registrationProduct.IdPromotion);
            ViewBag.IdRegistration = new SelectList(db.Registrations, "Id", "Description", registrationProduct.IdRegistration);
            return View(registrationProduct);
        }

        // GET: RegistrationProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationProduct registrationProduct = db.RegistrationProducts.Find(id);
            if (registrationProduct == null)
            {
                return HttpNotFound();
            }
            return View(registrationProduct);
        }

        // POST: RegistrationProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RegistrationProduct registrationProduct = db.RegistrationProducts.Find(id);
            db.RegistrationProducts.Remove(registrationProduct);
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
