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

namespace SuperbrainManagement.Controllers
{
    public class Schedules2Controller : Controller
    {
        private ModelDbContext db = new ModelDbContext();
        
        //class responser
        public class SchedulesRes
        {
            public string NameClass { get; set; }
            public string allSchedules { get; set; }
        }
        // GET: Schedules
        public ActionResult Index()
        {
            List<SchedulesRes> schedules = new List<SchedulesRes>();
            List<Class> classes = db.Classes.ToList();
            foreach (Class CLS in classes)
            {
                SchedulesRes schedulesRes = new SchedulesRes();
                schedulesRes.NameClass = CLS.Name;
                List<Schedule> schedules1 = Connect.Select<Schedule>("select * from Schedule where IdClass = '" + CLS.Id + "'");
                var listtkb = "";
                foreach (Schedule schedule in schedules1)
                {
                    if (schedule.Active == true)
                    {
                        listtkb = listtkb + lineweek(Convert.ToInt32(schedule.IdWeek));
                        listtkb += ",";
                    }

                }
                schedulesRes.allSchedules = listtkb;
                schedules.Add(schedulesRes);
            }
            Session["datasche"] = schedules;
            List<Schedule> schedules2 = Connect.Select<Schedule>("Select * from Schedule");
            Session["listEmployee"] = db.Employees.ToList();
            Session["listRoom"] = db.Rooms.ToList();
            return View(schedules2);
        }
        public string lineweek(int id)
        {
            switch (id)
            {
                case 0: return "Chủ nhật";
                case 1: return "2";
                case 2: return "3";
                case 3: return "4";
                case 4: return "5";
                case 5: return "6";
                case 6: return "7";
                default: return ""; // Không cần break sau return
            }
        }
        public static string line_week(int id)
        {
            switch (id)
            {
                case 0: return "Chủ nhật";
                case 1: return "Thứ 2";
                case 2: return "Thứ 3";
                case 3: return "Thứ 4";
                case 4: return "Thứ 5";
                case 5: return "Thứ 6";
                case 6: return "Thứ 7";
                default: return ""; // Không cần break sau return
            }
        }
        public static string ConvertToTimeString(DateTime time)
        {
            // Convert DateTime to string with the specified format
            string timeString = time.ToString("hh:mm tt");
            return timeString;
        }
        // GET: Schedules/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // GET: Schedules/Create
        public ActionResult Create()
        {
            ViewBag.IdClass = new SelectList(db.Classes, "Id", "Name");
            ViewBag.IdEmployee = new SelectList(db.Employees, "Id", "Name");
            ViewBag.IdRoom = new SelectList(db.Rooms, "Id", "Name");
            ViewBag.IdUser = new SelectList(db.Users, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult updateActive(List<Schedule> schedules)
        {
            if (schedules != null)
            {
                foreach (Schedule schedule in schedules)
                {
                    Schedule schedule1 = db.Schedules.FirstOrDefault(x => x.IdWeek == schedule.IdWeek && x.IdClass == schedule.IdClass);
                    if (schedule.Active != null)
                    {
                        schedule1.Active = schedule.Active;


                    }
                    schedule1.IdRoom = schedule.IdRoom;
                    schedule1.IdEmployee = schedule.IdEmployee;
                    db.SaveChanges();
                }
            }
            return Json(schedules);
        }
       
    }
}
