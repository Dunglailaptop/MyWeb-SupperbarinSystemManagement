using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.EnterpriseServices;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using SuperbrainManagement.Models;
using SuperbrainManagement.Views.ConfigView;

namespace SuperbrainManagement.Controllers
{
    public class UsersController : Controller
    {
        private ModelDbContext db = new ModelDbContext();
        //FUNCTION LOAD_DATA USERS
        public ActionResult Index()
        {
            return View();

        }
        [HttpGet]
        public ActionResult LoadData()
        {
            DataTable DataTableUsers = Connect.SelectAll("select ROW_NUMBER() OVER (ORDER BY users.id) AS stt,users.Id,users.Name,users.Username,users.Active,users.DateCreate from dbo.[User] users inner join Branch branchs on branchs.Id = users.IdBranch \r\n");
            StringBuilder htmlBuilder = new StringBuilder();
            foreach (DataRow row in DataTableUsers.Rows) {
                htmlBuilder.Append("<tr class=\"bordered-bottom\">");
                htmlBuilder.AppendFormat("<td>{0}</td>", row["stt"]);
                htmlBuilder.AppendFormat("<td>{0}</td>", row["Username"]);
                htmlBuilder.AppendFormat("<td>{0}</td>", row["Name"]);
                htmlBuilder.Append("<td></td>"); // Placeholder for your additional data
                htmlBuilder.Append("<td>1</td>"); // Placeholder for your additional data
                htmlBuilder.AppendFormat("<td>{0}</td>", row["DateCreate"]);
                htmlBuilder.Append("<td>");
                htmlBuilder.Append("<label class=\"custom-control ios-switch\">");
                htmlBuilder.AppendFormat("<input data-id=\"{0}\" value=\"{1}\" type=\"checkbox\" class=\"ios-switch-control-input\" onclick=\"ChangeStatus(this)\" {2}>",
                    row["Id"],
                    row["Active"] != DBNull.Value && Convert.ToBoolean(row["Active"]) ? 0 : 1,
                    row["Active"] != DBNull.Value && Convert.ToBoolean(row["Active"]) ? "checked" : "");
                htmlBuilder.Append("<span class=\"ios-switch-control-indicator\"></span>");
                htmlBuilder.Append("</label>");
                htmlBuilder.Append("</td>");
                var id = Convert.ToInt32(row["Id"]);
                var permissions = new List<string> { "Phân quyền" };
                htmlBuilder.Append("<td></td>");
                htmlBuilder.Append("<td>" + ConfigHtml.GenerateInlineList(Convert.ToInt32(row["Id"]),permissions)+"</td>"); // Placeholder for your additional data
                htmlBuilder.Append("</tr>");
            }
            return Json(htmlBuilder.ToString(),JsonRequestBehavior.AllowGet);
        }
        //FUNCTION GET DATA WITH PERMISSION ID
        [HttpGet]
        public ActionResult getDataWithIdPermission(int idUserAccount)
        {
          
            List<UserPermission> userPermissions = Connect.Select<UserPermission>("select * from UserPermission where IdUser = '" + idUserAccount + "'");
            List<object> data = new List<object>();
            if(userPermissions.Count > 0)
            {
               data = loaddatapermission(idUserAccount);
            }else
            {
                List<Permission> permissions = Connect.Select<Permission>("select * from Permission");
                // Create a new UserPermission object and set its properties
                foreach (Permission per in permissions)
                {
                    UserPermission newPermission = new UserPermission
                    {
                        IdUser = idUserAccount, // Assuming IdUser is a property in your UserPermission entity
                        IdPermission = per.Id, // Assuming IdPermission is a property in your UserPermission entity
                        IsRead = false,
                        IsDelete = false,
                        IsCreate = false,
                        IsEdit = false,
                    };
                    // Add the new permission to the DataContext
                    db.UserPermissions.Add(newPermission);
                    db.SaveChanges();
                    // Submit changes to persist the new record to the database

                }
                data = loaddatapermission(idUserAccount);
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public List<Object> loaddatapermission(int id)
        {
            List<PermissionCategory> permissionCategorys = Connect.Select<PermissionCategory>("select * from PermissionCategory");
            List<object> data = new List<object>();
            Session["iduseraccount"] = id;
            foreach (var permissionCategory in permissionCategorys)
            {

                var dataPermissionCategorywithlistper = new
                {
                    Name = permissionCategory.Name,
                    code = permissionCategory.Code,
                    Permissions = new List<Object>()
                };
                List<Permission> permissions = Connect.Select<Permission>("Select * from Permission where IdPermissionCategory='" + permissionCategory.Id + "'");
                foreach (var permission in permissions)
                {
                    UserPermission userPermission = Connect.SelectSingle<UserPermission>("select * from UserPermission where IdUser = '" + id + "' AND IdPermission = '" + permission.Id + "'");
                    var dataSingle = new
                    {

                        IsRead = userPermission.IsRead,
                        IsCreate = userPermission.IsCreate,
                        IsEdit = userPermission.IsEdit,
                        IsDelete = userPermission.IsDelete,
                        code = permission.Code,
                        Name = permission.Name,
                        id = userPermission.IdPermission

                    };
                    dataPermissionCategorywithlistper.Permissions.Add(dataSingle);
                }
                data.Add(dataPermissionCategorywithlistper);
            }
            return data;
     
        }

        [HttpPost]
        public ActionResult SaveChange(List<UserPermission> Permissions)
        {
            int id = Convert.ToInt32(Session["iduseraccount"]);
            List<UserPermission> users = Connect.Select<UserPermission>("select * from UserPermission where IdUser = '" + id + "'");
            if (users != null)
            {
                foreach (UserPermission userPermission in Permissions)
                {
                    UserPermission userPermissionUpdate = db.UserPermissions.FirstOrDefault(x => x.IdUser == id && x.IdPermission == userPermission.IdPermission);

                    userPermissionUpdate.IsRead = userPermission.IsRead;
                    userPermissionUpdate.IsDelete = userPermission.IsDelete;
                    userPermissionUpdate.IsCreate = userPermission.IsCreate;
                    userPermissionUpdate.IsEdit = userPermission.IsEdit;

                    db.SaveChanges();
                }
            }



            return View();
        }


        
        [HttpPost]
        public ActionResult updatestatus(int id, int status)
        {
            User user = db.Users.Find(id);
            if (user != null)
            {
                user.Active = status == 1 ? true : false;
                db.SaveChanges();
                return Json(user.Active);
            }
            else
            {
                return Json(false); // trả về false nếu không tìm thấy người dùng với id tương ứng
            }
        }

        [HttpGet]
         public ActionResult search(string keyword) 
        {
            DataTable DataTableUsers = Connect.SelectAll("select ROW_NUMBER() OVER (ORDER BY users.id) AS stt,users.Id,users.Name,users.Username,users.Active,users.DateCreate from dbo.[User] users inner join Branch branchs on branchs.Id = users.IdBranch where users.Name LIKE '%"+keyword+"%' \r\n");
            StringBuilder htmlBuilder = new StringBuilder();
            foreach (DataRow row in DataTableUsers.Rows)
            {
                htmlBuilder.Append("<tr class=\"bordered-bottom\">");
                htmlBuilder.AppendFormat("<td>{0}</td>", row["stt"]);
                htmlBuilder.AppendFormat("<td>{0}</td>", row["Username"]);
                htmlBuilder.AppendFormat("<td>{0}</td>", row["Name"]);
                htmlBuilder.Append("<td></td>"); // Placeholder for your additional data
                htmlBuilder.Append("<td>1</td>"); // Placeholder for your additional data
                htmlBuilder.AppendFormat("<td>{0}</td>", row["DateCreate"]);
                htmlBuilder.Append("<td>");
                htmlBuilder.Append("<label class=\"custom-control ios-switch\">");
                htmlBuilder.AppendFormat("<input data-id=\"{0}\" value=\"{1}\" type=\"checkbox\" class=\"ios-switch-control-input\" onclick=\"ChangeStatus(this)\" {2}>",
                    row["Id"],
                    row["Active"] != DBNull.Value && Convert.ToBoolean(row["Active"]) ? 0 : 1,
                    row["Active"] != DBNull.Value && Convert.ToBoolean(row["Active"]) ? "checked" : "");
                htmlBuilder.Append("<span class=\"ios-switch-control-indicator\"></span>");
                htmlBuilder.Append("</label>");
                htmlBuilder.Append("</td>");
                var id = Convert.ToInt32(row["Id"]);
                var permissions = new List<string> { "Phân quyền" };
                htmlBuilder.Append("<td></td>");
                htmlBuilder.Append("<td>" + ConfigHtml.GenerateInlineList(Convert.ToInt32(row["Id"]), permissions) + "</td>"); // Placeholder for your additional data
                htmlBuilder.Append("</tr>");
            }
            return Json(htmlBuilder.ToString(), JsonRequestBehavior.AllowGet);

        }

    }
}
