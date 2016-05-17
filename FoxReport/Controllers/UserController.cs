using FoxReport.Helper;
using FoxReport.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoxReport.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            List<UserInfo> userList = SqlDbHelper.GetUsersInfo();

            return View(userList);
        }
        [HttpPost]
        public JsonResult AddUser()
        {
            string userName = Request["userName"];
            string userRole = "0";
            string userId = SqlDbHelper.AddUser(userName, userRole);
            var result = new
            {
                OK = true,
                UserId = userId
            };
            return Json(result);
        }
        [HttpPost]
        public JsonResult EditUser()
        {
            string userId = Request["userId"];
            string userName = Request["userName"];
            bool ok = SqlDbHelper.EditUser(userId, userName);
            var result = new
            {
                OK = ok
            };
            return Json(result);
        }
        [HttpPost]
        public JsonResult DeleteUser()
        {
            string userId = Request["userId"];
            string userName = Request["userName"];
            bool ok = SqlDbHelper.DeleteUser(userId);
            var result = new
            {
                OK = ok
            };
            return Json(result);
        }
    }
}
