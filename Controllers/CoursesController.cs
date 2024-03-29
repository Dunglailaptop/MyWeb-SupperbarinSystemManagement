using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Provider;
using Newtonsoft.Json;
using SuperbrainManagement.Models;

namespace SuperbrainManagement.Controllers
{
    public class CoursesController : Controller
    {
        private ModelDbContext db = new ModelDbContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult loaddata()
        {
            List<Program> programs = Connect.Select<Program>("select * from Program");
            string str = "";

            foreach (Program program in programs)
            {
                str += "<tr class='bg-info-ltest'>"
                     + "<td colspan='8'>"
                     + "<div class='m-l'>"
                     + "<a href=''>"
                     + "<i class='bi bi-arrow-return-right'></i>"
                     + "<b>" + program.Name + "</b>"
                     + "</a>"
                     + "</div>"
                     + "</td>"
                     + "</tr>";

                List<Course> courses = Connect.Select<Course>("select * from Course where IdProgram='" + program.Id + "'");
                foreach (Course course in courses)
                {
                    str += "<tr>"
                         + "<td class='text-center'></td>"
                         + "<td><div class='m-l-lg'><i class='bi bi-arrow-return-right'></i>" + course.Name + "</div></td>"
                         + "<td>" + course.Name + "</td>"
                         + "<td>" + course.Price + "</td>"
                         + "<td>" + course.Levels + "</td>"
                         + "<td class='text-center'>"
                         + "<a href='javascript:Add_kho_khoahoc(\"105\")' class='btn btn-success btn-sm m-r-xs'>"
                         + "<i class='fa fa-pencil'></i> Tài Liệu"
                         + "</a>"
                         + "<a href='javascript:Edit_khoahoc(\"105\")' class='btn btn-dark btn-sm m-r-xs'>"
                         + "<i class='fa fa-pencil'></i> Sửa"
                         + "</a>"
                         + "<a href='javascript:del_khoahoc(\"105\")' class='btn btn-danger btn-sm'>"
                         + "<i class='fa fa-times'></i> Xóa"
                         + "</a>"
                         + "</td>"
                         + "</tr>";
                }
            }

            return Json(str,JsonRequestBehavior.AllowGet);
        }
    }
}

