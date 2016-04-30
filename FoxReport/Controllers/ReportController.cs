﻿using FoxReport.Helper;
using FoxReport.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoxReport.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/
        public ActionResult Index()
        {
            HttpCookie cookie = GetCookie();
            string userId = cookie.Values["userId"];
            string week = cookie.Values["week"];
            string isForeign = cookie.Values["isForeign"];
           
            string whereCondition = " where UserId='" + userId + "' and Week=" + week + " and IsForeign=" + isForeign +
                " limit 0, " + ConfigurationManager.AppSettings["pageSize"];
            int[] totalCount, totalPage;
            InitReport initReport = SqlDbHelper.GetInitReport(whereCondition, out totalCount, out totalPage);
            initReport.totalCount = totalCount;
            initReport.totalPage = totalPage;

            return PartialView(initReport);
        }
        [HttpPost]
        public JsonResult DeleteData(string id, string tableName)
        {
            int deleteCount = SqlDbHelper.DeleteData(tableName, id);
            var obj = new
            {
                DeleteCount = deleteCount
            };
            return Json(obj);
        }

        public ActionResult Search(string userId, string week, string isForeign)
        {
            HttpCookie cookie = new HttpCookie("SearchInfo");
            cookie.Values["userId"] = userId;           
            cookie.Values["isForeign"] = isForeign;
            cookie.Values["week"] = week;
            cookie.Expires = DateTime.Now.AddDays(30);
            HttpContext.Response.Cookies.Add(cookie);

            ViewBag.SelectedYearWeek = week;
            ViewBag.SelectedUserId = userId;
            ViewBag.SelectedIsForeign = isForeign;

            string whereCondition = " where UserId='" + userId + "' and Week=" + week + " and IsForeign=" + isForeign +
                " limit 0, " + ConfigurationManager.AppSettings["pageSize"];
            int[] totalCount, totalPage;
            InitReport initReport = SqlDbHelper.GetInitReport(whereCondition, out totalCount, out totalPage);
            return PartialView("Index", initReport);
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveContent(string id)
        {
            string userId = Request["userId"];
            string isForeign = Request["isForeign"];
            string week = Request["week"];
            string data = Request["content"];
            string[] ids = id.Split('_');
            string panel = ids[0];
            string tab = ids[1];
            string column = ids[2];
            string dataId = ids[3];
            int newId = SqlDbHelper.SaveText(panel + "_" + tab, column, data, dataId, userId, week, isForeign);
            var obj = new
            {
                NewId = newId
            };
            return Json(obj);
        }
        /// <summary>
        /// 一、整体概况，获取每个Tab内容
        /// </summary>
        /// <param name="id">表名称后面部分：TatgetStrategy/Version/Feedback/Suggest</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="userId">用户id</param>
        /// <param name="week">年周</param>
        /// <param name="isForeign">0国内，1国外</param>
        /// <returns></returns>
        public PartialViewResult Summary(string id, int pageIndex,string userId, string week, string isForeign)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            int totalCount = 0;
            int totalPage = 0;
            string whereCondition = " where UserId=" + userId + " and Week=" + week + " and IsForeign=" + isForeign + 
                " limit " + ((pageIndex - 1) * pageSize).ToString() + ", " + pageSize.ToString() ;
                
            if (id == "Version")
            {
                List<SummaryVersion> versionList = SqlDbHelper.GetSummaryVersion(whereCondition, out totalCount, out totalPage);
                return PartialView("_Summary_" + id, versionList);
            }
            else if (id == "Feedback")
            {
                List<SummaryFeedback> feedbackList = SqlDbHelper.GetSummaryFeedback(whereCondition, out totalCount, out totalPage);
                return PartialView("_Summary_" + id, feedbackList);
            }
            else if (id == "Suggest")
            {
                List<SummarySuggest> suggestList = SqlDbHelper.GetSummarySuggest(whereCondition, out totalCount, out totalPage);
                ViewBag.TotalCount = totalCount;
                ViewBag.TotalPage = totalPage;
                return PartialView("_Summary_" + id, suggestList);
            }
            else// "TargetStrategy"
            {
                List<SummaryTargetStrategy> targetList = SqlDbHelper.GetSummaryTargetStrategy(whereCondition, out totalCount, out totalPage);
                return PartialView("_Summary_" + id, targetList);
            }
        }
        /// <summary>
        /// 二、项目概况，获取每个Tab内容
        /// </summary>
        /// <param name="id">Project_Info表的列名称：Target/Progress/Teamwork/VersionDetail/VersionQuality</param>
        /// <param name="recordId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="userId"></param>
        /// <param name="week"></param>
        /// <param name="isForeign"></param>
        /// <returns></returns>
        public PartialViewResult ProjectInfo(string id, int recordId, string userId, string week, string isForeign)
        {
            string whereCondition = " where id=" + recordId.ToString(); // UserId=" + userId + " and Week=" + week + " and IsForeign=" + isForeign;
                
            ViewBag.Column = id;
            ViewBag.Id = recordId;
            string content = "";
            ProjectInfo p = SqlDbHelper.GetProjectInfo(whereCondition);
            
            switch (id)
            {
                case "Progress":
                    content = p.Progress;
                    break;
                case "Target":
                    content = p.Target;
                    break;
                case "Teamwork":
                    content = p.Teamwork;
                    break;
                case "VersionDetail":
                    content = p.VersionDetail;
                    break;
                case "VersionQuality":
                    content = p.VersionQuality;
                    break;
            }
           
            ViewBag.Content = content;
            return PartialView("_Project_Info", p);
        }
        //生成查询Div的html
        public ActionResult SearchDiv()
        {
            HttpCookie cookie = GetCookie();
            ViewBag.SelectedYearWeek = cookie.Values["week"];
            ViewBag.SelectedUserId = cookie.Values["userId"];
            ViewBag.SelectedIsForeign = cookie.Values["isForeign"];

            UserAndWeekInfo userWeek = new UserAndWeekInfo();
            return PartialView("_SearchDiv", userWeek);
        }

        private HttpCookie GetCookie()
        {
            HttpCookie cookie = HttpContext.Request.Cookies["SearchInfo"];
            if (cookie == null)
            {
                cookie = new HttpCookie("SearchInfo");
                
                cookie.Values["week"] = DateTime.Now.Year.ToString() + WeekHelper.GetWeekOfYear(DateTime.Now).ToString();
                cookie.Values["userId"] = CacheFoxData.UserList.Count > 0 ? CacheFoxData.UserList[0].UserId : "";
                cookie.Values["isForeign"] = "0";                
            }
            cookie.Expires = DateTime.Now.AddDays(30);
            HttpContext.Response.Cookies.Add(cookie);

            return cookie;
        }
    }
}
