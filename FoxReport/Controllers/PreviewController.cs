using FoxReport.Helper;
using FoxReport.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoxReport.Controllers
{
    public class PreviewController : Controller
    {
        //
        // GET: /Preview/

        public ActionResult Index(int id)
        {
            HttpCookie cookie = GetCookie();
            string userId = cookie.Values["userId"];
            string week = cookie.Values["week"];
            string isForeign = cookie.Values["isForeign"];

            string whereCondition = " where UserId='" + userId + "' and Week=" + week + " and IsForeign=" + isForeign;
            string limit = " "; //" limit 0, 1000";
            int[] totalCount, totalPage;
            int t, p;
            InitReport initReport = SqlDbHelper.GetInitReport(whereCondition, limit, out totalCount, out totalPage);
            ReportData report = new ReportData();
            report.AffairProductList = initReport.AffairProductList;
            report.assistInfo = initReport.assistInfo;
            report.ProjectInfoList = initReport.ProjectInfoList;
            report.ReportName = initReport.ReportName;
            report.SummaryTargetStrategyList = initReport.SummaryTargetStrategyList;
            report.SummaryFeedbackList = SqlDbHelper.GetSummaryFeedback(whereCondition, limit, out t, out p);
            report.SummaryVersionList = SqlDbHelper.GetSummaryVersion(whereCondition, limit, out t, out p);
            report.SummarySuggestList = SqlDbHelper.GetSummarySuggest(whereCondition, limit, out t, out p);
            report.teamworkInfo = initReport.teamworkInfo;
            return View(report);
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
