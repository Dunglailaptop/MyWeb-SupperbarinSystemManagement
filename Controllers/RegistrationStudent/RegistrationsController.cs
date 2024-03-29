using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SuperbrainManagement.Models;
using System.EnterpriseServices.CompensatingResourceManager;

namespace SuperbrainManagement.Controllers.RegistrationStudent
{
    public class RegistrationsController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

       public ActionResult RegistrationListView()
        {

            return View();
        }
        [HttpGet]
        public ActionResult RegistrationListWithIdStudent(int IdStudent)
        {
            DataTable DataTablestudents = Connect.SelectAll("select res.DateCreate, cour.Name, res.Code, cour.Price, rescourse.Status  from Registration res\r\ninner join RegistrationCourse rescourse\r\non rescourse.IdRegistration = res.Id\r\ninner join Course cour\r\non cour.Id = rescourse.IdCourse\r\nwhere res.IdStudent = '" + IdStudent + "'");

            var str = "";
            foreach (DataRow row in DataTablestudents.Rows)
            {
              
               
                 
                
                str += "<tr>"
                    + "<th>" + row["Code"] + "</th>"
                    + "<th>" + row["Name"] + "</th>"
                    + "<th>" + row["Price"] + "</th>"
                    + "<th>" + row["Status"] + "</th>"
                    + "<th>" + Convert.ToDateTime(row["DateCreate"]).ToString("dd/MM/yyyy") + "</th>"
                    + "</tr>";
            }
            var item = new
            {
                str
            };
            return Json(item, JsonRequestBehavior.AllowGet);
        }

    }
}
