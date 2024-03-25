using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SuperbrainManagement.Models;

namespace SuperbrainManagement.Controllers
{
    public class StudentsController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Students
        public ActionResult Index()
        {
            
          return View();
        }

      
        public ActionResult loaddata()
        {
           List<Student> st = Connect.Select<Student>("select * from Student");
            return Json(st, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(string keyword)
        {
            // Tạo chuỗi truy vấn SQL hoàn chỉnh với giá trị keyword được định dạng đúng
            string query = "SELECT * FROM Student WHERE Name COLLATE Latin1_General_CI_AI LIKE N'%" + keyword + "%'";

            // Thực hiện truy vấn SQL
            List<Student> students = Connect.Select<Student>(query);

            // Trả về kết quả dưới dạng JSON
            return Json(students, JsonRequestBehavior.AllowGet);
        }
        public class students
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Code { get; set; }
            public string Phone { get; set; }
        }
        public ActionResult savechange(students student)
        {
            Student student1 = new Student();
            student1.Name = student.Name;
            student1.Email = student.Email;
            student1.Phone = student.Phone;
            student1.Code = student.Code;
            db.Students.Add(student1);
            db.SaveChanges();
            return Json(student1,JsonRequestBehavior.AllowGet);
        }


    }
}
