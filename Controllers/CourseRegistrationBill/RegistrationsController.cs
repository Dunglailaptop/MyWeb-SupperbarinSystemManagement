using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using DevExtreme.AspNet.Mvc.Builders;
using Newtonsoft.Json;
using SuperbrainManagement.Models;
using static SuperbrainManagement.Controllers.CourseTrainBill.RegistrationsController;

namespace SuperbrainManagement.Controllers.CourseTrainBill
{
    public class RegistrationsController : Controller
    {
        private ModelDbContext db = new ModelDbContext();
        public class RegistrationResponse
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
            public DateTime date { get; set; }
        }
        // GET: Registrations
        public ActionResult Index(int id)
        {
            Student student = db.Students.Where(x => x.Id == id).FirstOrDefault();
            RegistrationResponse response = new RegistrationResponse();
            response.Id = GenerateCode();
            response.Name = student.Name;
            response.Code = student.Code;
            response.date = DateTime.Now;
            Session["ListProgram"] = db.Programs.ToList();
            Session["ListCourse"] = db.Courses.ToList();
            Session["promotion"] = db.Promotions.ToList();
            Session["infoUser"] = student;
            return View();
        }
        public string GenerateCode()
        {
            string prefix = "DK_HQ";

            // Generate a random number
            Random random = new Random();
            int randomNumber = random.Next(1000, 9999); // Adjust range as needed

            // Generate the new code
            string newCode = $"{prefix}_{randomNumber}";

            return newCode;
        }

        public class cartitem
        {
            public int idProgram { get; set; }
            public int idCourse { get; set; }
            public int Idpromotion { get; set; }
            public int price { get; set; }
            public string nameprogram { get; set; }
            public string IdRegistrations {  get; set; }

            public string DateTime { get; set; }
            public int Id { get; set; }
            public int total { get; set; }
            
        }
        public ActionResult getData(string IdRegistration)
        {
            DataTable dataTable = Connect.SelectAll("select cour.Name as NameCourse,rescourse.IdCourse,res.Id,rescourse.Price,pro.Name as NameProgram,res.Amount,rescourse.TotalAmount,res.Code,res.DateCreate  from Registration res \r\ninner join RegistrationCourse rescourse on rescourse.IdRegistration = res.Id\r\ninner join Course cour on cour.Id = rescourse.IdCourse\r\ninner join Program pro on pro.Id = cour.IdProgram where res.Id = '" + IdRegistration + "'");
            Registration registration = Connect.SelectSingle<Registration>("select * from Registration where Id = '"+IdRegistration+"'");
            // Khởi tạo danh sách HTML string để lưu dữ liệu
            var data = new StringBuilder();
            var totalamount = 0;
            var i = 0;
            var datacreate = "";
            var idRegistration = 0;
            var code = "";
            foreach (DataRow row in dataTable.Rows)
            {
                i++;
                string amountString = string.Format("{0:N0} VND", row["TotalAmount"]);
                string nameProgramCourse = row["NameProgram"].ToString() + "<hr>" + row["NameCourse"].ToString();
                // Tạo dòng HTML cho mỗi dòng dữ liệu
                var newRow = "<tr>" +
                    "<td style='text-align:center;'>" + i + "</td>" +
                    "<td style='text-align:left;'>" + nameProgramCourse + "</td>" +
                    "<td style='text-align:center;'>1</td>" +
                    "<td style='text-align:right;'>" + amountString + "</td>" +
                    "<td style='text-align:center;'>0</td>" +
                    "<td style='text-align:right;'>" + amountString + "</td>" +
                    "<td style='text-align:center;'>" +
                    "<a href='#' class='btn btn-sm btn-danger delete-btn' onclick=\"deleteitem('" + row["IdCourse"] + "','" + row["Id"] + "')\" data-courseid='" + row["IdCourse"] + "' data-registrationid='" + row["Id"] + "'>" +
                    "<i class='bx bx-trash-alt font-size-18'></i>" +
                    "</a>" +
                    "</td>" +
                    "</tr>";

                // Thêm dòng vào danh sách HTML
                data.Append(newRow);

                // Lấy ngày tạo
              
                // Tính tổng số tiền
              
                totalamount += Convert.ToInt32(row["TotalAmount"]);
            }

            // Tạo đối tượng kết quả
            var result = new
            {
                datalist = data.ToString(), // Chuyển đổi danh sách HTML thành chuỗi
                TotalAmount = totalamount,
                DateCreate = Convert.ToDateTime(registration.DateCreate).ToString("dd/MM/yyyy"),
                idRegistrations = registration.Id,
                Code = registration.Code
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult saveCookie(int idProgram,int IdCourse,int Idpromotion,int price,string nameprogram,string code)
        {
            MD5Hash md5 = new MD5Hash();
            string iduser = System.Web.HttpContext.Current.Request.Cookies["check"]["iduser"].ToString();
            iduser = md5.Decrypt(iduser.ToString());
            Student student = Session["infoUser"] as Student;
            Registration registrations = Connect.SelectSingle<Registration>("select * from Registration where Id='"+code+"'");
            List<cartitem> cartitems = new List<cartitem>(); 
            if(registrations != null)
            {
                RegistrationCourse registrationCourse = Connect.SelectSingle<RegistrationCourse>("select * from RegistrationCourse where IdRegistration = '"+registrations.Id+"' and IdCourse = '"+IdCourse+"'");
                if(registrationCourse != null)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }else
                {
                    RegistrationCourse courseres = new RegistrationCourse();
                    courseres.IdCourse = IdCourse;
                    courseres.IdRegistration = registrations.Id;
                    courseres.Price = price;    
                    courseres.TotalAmount = price;
                    courseres.Amount = price;
                    courseres.Status = true;
                    db.RegistrationCourses.Add(courseres);
                    db.SaveChanges();
                    Course course = db.Courses.Where(x => x.Id == IdCourse).FirstOrDefault();
                    Program program = db.Programs.Where(x => x.Id == course.IdProgram).FirstOrDefault();
                    cartitem cartitem = new cartitem();
                    cartitem.idProgram = idProgram;
                    cartitem.idCourse = IdCourse;
                    cartitem.price = Convert.ToInt32(courseres.Price);
                    cartitem.Id = registrations.Id;
                    cartitem.nameprogram = program.Name;
                    cartitem.IdRegistrations = registrations.Code;
                    cartitem.DateTime = DateTime.Now.ToString("dd/MM/yyyy");
                    List<RegistrationCourse> registrationtotal = Connect.Select<RegistrationCourse>("select * from RegistrationCourse where IdRegistration = '"+registrations.Id+"'");
                    cartitem.total = Convert.ToInt32(registrationtotal.Sum(x=>x.Amount));

                    cartitems.Add(cartitem);

                    int total = cartitems.Sum(item => item.price);
                    Console.WriteLine(cartitems);
                    return Json(registrations.Id, JsonRequestBehavior.AllowGet);
                }
                
            }else
            {
                Registration registration = new Registration(); 
                registration.IdBranch = student.IdBranch;
                registration.IdStudent = student.Id;
                registration.IdUser = Convert.ToInt32(iduser);
                registration.Amount = price;
                registration.TotalAmount = price;
                registration.Discount = price;
                registration.Code = GenerateCode();
                registration.DateCreate = DateTime.Now;
                db.Registrations.Add(registration);
                db.SaveChanges();
                if(registration.Id != null)
                {
                    RegistrationCourse registrationCourse = new RegistrationCourse();
                    registrationCourse.IdCourse = IdCourse;
                    registrationCourse.IdRegistration = registration.Id;
                    registrationCourse.Amount = price;
                    registrationCourse.TotalAmount = price;
                    registrationCourse.Discount = price;    
                    registrationCourse.DateExtend = DateTime.Now;
                    db.RegistrationCourses.Add(registrationCourse);
                    db.SaveChanges();
                }
                Course course = db.Courses.Where(x=>x.Id == IdCourse).FirstOrDefault();
                Program program = db.Programs.Where(x => x.Id == course.IdProgram).FirstOrDefault();
                cartitem cartitem = new cartitem();
                cartitem.idProgram = idProgram;
                cartitem.idCourse = IdCourse;
                cartitem.price = price;
               
                cartitem.nameprogram = program.Name;
                cartitem.IdRegistrations = GenerateCode();
                cartitem.DateTime = DateTime.Now.ToString("dd/MM/yyyy");
                cartitem.Id = registration.Id;
                List<RegistrationCourse> registrationtotal = Connect.Select<RegistrationCourse>("select * from RegistrationCourse where IdRegistration = '" + registration.Id + "'");
                cartitem.total = Convert.ToInt32(registrationtotal.Sum(x => x.Amount));
                cartitems.Add(cartitem);
                
                int total = cartitems.Sum(item => item.price);
                Console.WriteLine(cartitems);
                return Json(registration.Id, JsonRequestBehavior.AllowGet);
            }
            //// Lấy danh sách sản phẩm từ cookie
            //List<cartitem> cartItems = GetCartItemsFromCookie();

            //// Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng hay chưa
            //cartitem existingItem = cartItems.FirstOrDefault(item => item.idCourse == IdCourse && item.idProgram == idProgram);
            //if(existingItem != null)
            //{
            //    return Json(new { error = "Da ton tai san pham" });
            //}
            //else
            //{
            //    cartitem cartitem = new cartitem();
            //    cartitem.idProgram = idProgram;
            //    cartitem.idCourse = IdCourse;
            //    cartitem.price = price;
            //    cartitem.Idpromotion = Idpromotion;
            //    cartitem.nameprogram = nameprogram;
            //    cartitem.IdRegistrations = GenerateCode();
            //    cartitem.DateTime = DateTime.Now.ToString("dd/MM/yyyy");
            //    cartItems.Add(cartitem);
            //    SaveCartItemsToCookie(cartItems);
            //    int total = cartItems.Sum(item => item.price);
            //    Console.WriteLine(cartItems);

            //}
            //return Json(cartItems, JsonRequestBehavior.AllowGet);
        }
        private List<cartitem> GetCartItemsFromCookie()
        {
            // Lấy giá trị từ cookie
            var json = Request.Cookies["CartItems7"]?.Value;

            if (!string.IsNullOrEmpty(json))
            {
                // Chuyển chuỗi JSON thành danh sách các đối tượng
                return JsonConvert.DeserializeObject<List<cartitem>>(json);
            }

            // Nếu cookie không tồn tại hoặc trống, trả về danh sách rỗng
            return new List<cartitem>();
        }

        private void SaveCartItemsToCookie(List<cartitem> cartItems)
        {
            // Chuyển danh sách sản phẩm thành chuỗi JSON
            var json = JsonConvert.SerializeObject(cartItems);

            // Lưu chuỗi JSON vào cookie
            Response.Cookies["CartItems7"].Value = json;
        }
        [HttpPost] // Use POST for actions that modify data
        public ActionResult Deletes(int IdCourse, int IdRegistration)
        {
            try
            {
                var deleteItem = db.RegistrationCourses.FirstOrDefault(x => x.IdCourse == IdCourse && x.IdRegistration == IdRegistration);
                if (deleteItem != null)
                {
                    db.RegistrationCourses.Remove(deleteItem);
                    db.SaveChanges();
                    return Json(deleteItem.IdRegistration);

                }
                else
                {
                    return Json(new { success = false, message = "Item not found" });
                }
            }catch (Exception ex)
            {
                return Json(new { success = false, message = "Item not found" });
            }
        }


        // Define a DTO class for Course
        public class CourseDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal? price { get; set; }
            // Other properties if needed
        }

        // Use DTOs in your controller action
        public ActionResult GetDataCourse(int id)
        {
            List<CourseDTO> courseDTO = new List<CourseDTO>();
            if (id == 0)
            {
                var firstProgram = db.Programs.FirstOrDefault(); // Get the first program
                if (firstProgram != null)
                {
                    var courset = db.Courses.Where(x => x.IdProgram == firstProgram.Id).ToList();
                    foreach (var item in courset)
                    {
                        CourseDTO couse = new CourseDTO();
                        couse.Id = item.Id;
                        couse.Name = item.Name;
                        couse.price = item.Price;
                        courseDTO.Add(couse);
                    }
                }
            }
            else
            {
                var courses = db.Courses.Where(x => x.IdProgram == id).ToList();
                foreach (var item in courses)
                {
                    CourseDTO course = new CourseDTO();
                    course.Id = item.Id;
                    course.Name = item.Name;
                    course.price = item.Price;
                    courseDTO.Add(course);
                }
            }

            return Json(courseDTO, JsonRequestBehavior.AllowGet);
        }


        // GET: Registrations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // GET: Registrations/Create
        public ActionResult Create()
        {
            ViewBag.IdBranch = new SelectList(db.Branches, "Id", "Logo");
            ViewBag.IdStudent = new SelectList(db.Students, "Id", "Name");
            ViewBag.IdUser = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: Registrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DateCreate,IdUser,IdBranch,Amount,Discount,TotalAmount,Enable,Status,Description,IdStudent,IdCoupon")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                db.Registrations.Add(registration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdBranch = new SelectList(db.Branches, "Id", "Logo", registration.IdBranch);
            ViewBag.IdStudent = new SelectList(db.Students, "Id", "Name", registration.IdStudent);
            ViewBag.IdUser = new SelectList(db.Users, "Id", "Name", registration.IdUser);
            return View(registration);
        }

        // GET: Registrations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdBranch = new SelectList(db.Branches, "Id", "Logo", registration.IdBranch);
            ViewBag.IdStudent = new SelectList(db.Students, "Id", "Name", registration.IdStudent);
            ViewBag.IdUser = new SelectList(db.Users, "Id", "Name", registration.IdUser);
            return View(registration);
        }

        // POST: Registrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateCreate,IdUser,IdBranch,Amount,Discount,TotalAmount,Enable,Status,Description,IdStudent,IdCoupon")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdBranch = new SelectList(db.Branches, "Id", "Logo", registration.IdBranch);
            ViewBag.IdStudent = new SelectList(db.Students, "Id", "Name", registration.IdStudent);
            ViewBag.IdUser = new SelectList(db.Users, "Id", "Name", registration.IdUser);
            return View(registration);
        }

        // GET: Registrations/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Registration registration = db.Registrations.Find(id);
        //    if (registration == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(registration);
        //}

        // POST: Registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Registration registration = db.Registrations.Find(id);
            db.Registrations.Remove(registration);
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
