using FoxReport.Helper;
using FoxReport.Models;
using MySql.Data.MySqlClient;
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
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            int userId = 0;
            int isForeign = 0;
            int week = 0;
            InitShow initShow = SqlDbHelper.GetInitShow(userId, week, isForeign);
            return View(initShow);
        }

        public FileResult Download(int id)
        {
            string tempFileName = string.Format("{0}.docx", Guid.NewGuid().ToString());
            string absolutePath = Path.Combine(Path.GetTempPath(), tempFileName);
            Uri uri = new Uri(Request.Url.GetLeftPart(UriPartial.Authority)).Append("Preview/Index/" + id.ToString());
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

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveContent(string id, string content)
        {
            string data = content;
            string[] ids = id.Split('_');
            string panel = ids[0];
            string tab = ids[1];
            string column = ids[2];
            string dataId = ids[3];
            int newId = SqlDbHelper.SaveText(panel + "_" + tab, column, data, dataId);
            var obj = new
            {
                NewId = newId
            };
            return Json(obj);
        }

        public PartialViewResult Summary(string id)
        {
            int userId = 0;
            int isForeign = 0;
            int week = 0;
            if(id == "Version")
            {
                List<SummaryVersion> versionList = SqlDbHelper.GetSummaryVersion(userId, week, isForeign);
                return PartialView("_Summary_" + id, versionList);
            }
            else if (id == "Feedback")
            {
                List<SummaryFeedback> feedbackList = SqlDbHelper.GetSummaryFeedback(userId, week, isForeign);
                return PartialView("_Summary_" + id, feedbackList);
            }
            else// "TargetStrategy"
            {
                List<SummaryTargetStrategy> targetList = SqlDbHelper.GetSummaryTargetStrategy(userId, week, isForeign);
                return PartialView("_Summary_" + id, targetList);
            }
        }
        public PartialViewResult ProjectInfo(string id, int recordId)
        {
            int userId = 0;
            int isForeign = 0;
            int week = 0;
            ViewBag.Column = id;
            ViewBag.Id = recordId;
            string content = "";
            List<ProjectInfo> projectInfoList = SqlDbHelper.GetProjectInfo(userId, week, isForeign);
            foreach (ProjectInfo p in projectInfoList)
            {
                if (p.Id == recordId)
                {
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
                }
            }
            ViewBag.Content = content;
            return PartialView("_Project_Info", projectInfoList);
        }
        public PartialViewResult Detail(string id)
        {
            SummaryTargetStrategy t = new SummaryTargetStrategy();
            t.Id = 1;
            t.ProjectName = "ProjectName 1 id=" + id.ToString();
            t.Status = "Status";
            t.Strategy = "Strategy 1";
            t.Target = "Target";
            return PartialView("_Detail_" + id, t);
        }
        public PartialViewResult Problem(string id)
        {
            SummaryTargetStrategy t = new SummaryTargetStrategy();
            t.Id = 1;
            t.ProjectName = "ProjectName problem id=" + id.ToString();
            t.Status = "Status";
            t.Strategy = "Strategy 1";
            t.Target = "Target";
            return PartialView("_Problem_" + id, t);
        }
    }
}
