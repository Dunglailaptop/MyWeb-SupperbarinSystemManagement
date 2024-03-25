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
    public class RevenueReferencesController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: RevenueReferences
        public ActionResult Index()
        {
        
            return View();
        }
        public ActionResult getData()
        {
            DataTable dataTable = Connect.SelectAll("select course.IdCourse,res.Id,res.Code from Registration res inner join RegistrationCourse course on course.IdRegistration = res.Id");
            // Chuyển đổi DataTable thành danh sách các đối tượng đơn giản
            List<object> data = new List<object>();
            foreach (DataRow row in dataTable.Rows)
            {
                var rowData = new
                {
                    IdCourse = row["IdCourse"],
                    Id = row["Id"],
                    Code = row["Code"]
                    // Thêm các cột khác tương ứng
                };
                data.Add(rowData);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        // GET: RevenueReferences/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RevenueReference revenueReference = db.RevenueReferences.Find(id);
            if (revenueReference == null)
            {
                return HttpNotFound();
            }
            return View(revenueReference);
        }

        // GET: RevenueReferences/Create
        public ActionResult Create()
        {
            ViewBag.IdBranch = new SelectList(db.Branches, "Id", "Logo");
            ViewBag.IdUser = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: RevenueReferences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,Name,Price,Discount,StatusDiscount,DateCreate,IdUser,IdBranch,IsPublic")] RevenueReference revenueReference)
        {
            if (ModelState.IsValid)
            {
                db.RevenueReferences.Add(revenueReference);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdBranch = new SelectList(db.Branches, "Id", "Logo", revenueReference.IdBranch);
            ViewBag.IdUser = new SelectList(db.Users, "Id", "Name", revenueReference.IdUser);
            return View(revenueReference);
        }

        // GET: RevenueReferences/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RevenueReference revenueReference = db.RevenueReferences.Find(id);
            if (revenueReference == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdBranch = new SelectList(db.Branches, "Id", "Logo", revenueReference.IdBranch);
            ViewBag.IdUser = new SelectList(db.Users, "Id", "Name", revenueReference.IdUser);
            return View(revenueReference);
        }

        // POST: RevenueReferences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,Name,Price,Discount,StatusDiscount,DateCreate,IdUser,IdBranch,IsPublic")] RevenueReference revenueReference)
        {
            if (ModelState.IsValid)
            {
                db.Entry(revenueReference).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdBranch = new SelectList(db.Branches, "Id", "Logo", revenueReference.IdBranch);
            ViewBag.IdUser = new SelectList(db.Users, "Id", "Name", revenueReference.IdUser);
            return View(revenueReference);
        }

        // GET: RevenueReferences/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RevenueReference revenueReference = db.RevenueReferences.Find(id);
            if (revenueReference == null)
            {
                return HttpNotFound();
            }
            return View(revenueReference);
        }

        // POST: RevenueReferences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RevenueReference revenueReference = db.RevenueReferences.Find(id);
            db.RevenueReferences.Remove(revenueReference);
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
