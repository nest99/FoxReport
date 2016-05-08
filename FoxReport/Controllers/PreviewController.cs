using FoxReport.Helper;
using FoxReport.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoxReport.Controllers
{
    public class PreviewController : Controller
    {
        //
        // GET: /Preview/

        public ActionResult Index()
        {
            HttpCookie cookie = GetCookie();
            string userId = cookie.Values["userId"];
            string week = cookie.Values["week"];
            string isForeign = cookie.Values["isForeign"];

            string whereCondition = " where UserId='" + userId + "' and Week=" + week;
            string limit = " "; //" limit 0, 1000";            
            int t, p;
            
            ReportData report = new ReportData();
            report.ReportName = SqlDbHelper.GetReportName(whereCondition);
            report.AffairProductList = SqlDbHelper.GetAffairProduct(whereCondition, limit, null, out t, out p);
            report.assistInfo = SqlDbHelper.GetAssistInfo(whereCondition);

            report.ProjectInfoList = SqlDbHelper.GetProjectInfoList(whereCondition, limit, null, out t, out p);            
            report.SummaryTargetStrategyList = SqlDbHelper.GetSummaryTargetStrategy(whereCondition, limit, null, out t, out p);
            report.SummaryVersionList = SqlDbHelper.GetSummaryVersion(whereCondition, limit, null, out t, out p);

            report.SummaryFeedbackList = SqlDbHelper.GetSummaryFeedback(whereCondition, limit, null, out t, out p);            
            report.SummarySuggestList = SqlDbHelper.GetSummarySuggest(whereCondition, limit, null, out t, out p);
            report.teamworkInfo = SqlDbHelper.GetTeamworkInfo(whereCondition);
            return View(report);
        }

        public FileResult Download()
        {
            string tempFileName = string.Format("{0}.docx", Guid.NewGuid().ToString());
            string absolutePath = Path.Combine(Path.GetTempPath(), tempFileName);
            Uri uri = new Uri(Request.Url.GetLeftPart(UriPartial.Authority)).Append("Preview/Index/0"); //  /" + id.ToString()
            string baseDirectory = Directory.GetParent(Request.PhysicalApplicationPath).ToString();
            NameValueCollection headers = Request.Headers;
            try
            {
                string file = DocumentModel.Load(uri.ToString(), baseDirectory, new Identity
                {
                    Headers = headers
                }).SaveAs(absolutePath);
            }
            catch (Exception ex)
            {

            }

            return File(absolutePath, MimeMapping.GetMimeMapping(".docx"), "LoadFileName.docx");
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
