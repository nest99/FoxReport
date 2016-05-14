using FoxReport.Helper;
using FoxReport.Models;
using MySql.Data.MySqlClient;
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

            string whereCondition = " where Week=" + week;
            if (userId != "all")
            {
                whereCondition += " and UserId='" + userId + "' ";
            }
            string whereConditionForeign = whereCondition + " and IsForeign=" + isForeign;
            string limit = " limit 0, " + ConfigurationManager.AppSettings["pageSize"];
            
            int c1, c2, c3, c4, c5, p1, p2, p3, p4, p5;
            InitReport initReport = new InitReport();
            //initReport.ReportName = SqlDbHelper.GetReportName(whereCondition);

            initReport.SummaryTargetStrategyList = SqlDbHelper.GetSummaryTargetStrategy(whereConditionForeign, limit, null, out c1, out p1);
            initReport.ProjectInfoList = SqlDbHelper.GetProjectInfoList(whereConditionForeign, limit, null, out c2, out p2);

            initReport.AffairProductList = SqlDbHelper.GetAffairProduct(whereCondition, limit, null, out c3, out p3);
            initReport.TeamworkInfoList = SqlDbHelper.GetTeamworkInfoList(whereCondition, limit, null, out c4, out p4);
            initReport.AssistInfoList = SqlDbHelper.GetAssistInfoList(whereCondition, limit, null, out c5, out p5);
            int[] totalCount = { c1, c2, c3, c4, c5 };
            int[] totalPage = { p1, p2, p3, p4, p5 };
            initReport.totalCount = totalCount;
            initReport.totalPage = totalPage;
            ViewBag.UserList = CacheFoxData.UserList;            
            return PartialView(initReport);
        }
        public ActionResult Search(string userId, string week, string isForeign, string project)
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
            ViewBag.UserList = CacheFoxData.UserList;
            string whereCondition = " where Week=" + week;
            if (userId != "all")
            {
                whereCondition += " and UserId='" + userId + "' ";
            }
            string whereConditionForeign = whereCondition + " and IsForeign=" + isForeign;
            string whereConditionProject = whereConditionForeign;
            MySqlParameter[] parameters = null;
            if(!string.IsNullOrWhiteSpace(project))
            {
                whereConditionProject += " and ProjectName like @project ";                
                parameters = new MySqlParameter[1];
                parameters[0] = GetProjectNameParam(project);
            }

            string limit = " limit 0, " + ConfigurationManager.AppSettings["pageSize"];

            int c1, c2, c3, c4, c5, p1, p2, p3, p4, p5;
            InitReport initReport = new InitReport();
            //initReport.ReportName = SqlDbHelper.GetReportName(whereCondition);

            initReport.SummaryTargetStrategyList = SqlDbHelper.GetSummaryTargetStrategy(whereConditionProject, limit, parameters, out c1, out p1);
            initReport.ProjectInfoList = SqlDbHelper.GetProjectInfoList(whereConditionProject, limit, parameters, out c2, out p2);

            initReport.AffairProductList = SqlDbHelper.GetAffairProduct(whereCondition, limit, parameters, out c3, out p3);
            initReport.TeamworkInfoList = SqlDbHelper.GetTeamworkInfoList(whereCondition, limit, parameters, out c4, out p4);
            initReport.AssistInfoList = SqlDbHelper.GetAssistInfoList(whereCondition, limit, parameters, out c5, out p5);
            int[] totalCount = { c1, c2, c3, c4, c5 };
            int[] totalPage = { p1, p2, p3, p4, p5 };
            initReport.totalCount = totalCount;
            initReport.totalPage = totalPage;

            return PartialView("Index", initReport);
        }
        private MySqlParameter GetProjectNameParam(string project)
        {
            MySqlParameter p = new MySqlParameter("@project", MySqlDbType.VarString, 50);
            p.Value = "%" + project + "%";
            return p;
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

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveContent(string id) //保存富文本字段的值
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

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveColumnTextValue(string id) //保存纯文本字段的值
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
            int newId = SqlDbHelper.SaveColumnText(panel + "_" + tab, column, data, dataId, userId, week, isForeign);
            var obj = new
            {
                NewId = newId
            };
            return Json(obj);
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveReportName()
        {
            string userId = Request["userId"];
            string isForeign = Request["isForeign"];
            string week = Request["week"];
            string data = Request["content"];
            
            int result = SqlDbHelper.SaveReportName(data, userId, week, isForeign);
            var obj = new
            {
                Result = result
            };
            return Json(obj);
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveSeq()
        {
            string recordId = Request["recordId"];
            string seqTable = Request["seqTable"];
            string seq = Request["seq"];
            int s;
            int newId = -1; //seq不是数字
            if (int.TryParse(seq, out s))
            {
                newId = SqlDbHelper.SaveSeq(seqTable, recordId, seq);                
            }
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
        public PartialViewResult Summary(string id, int pageIndex,string userId, string week, string isForeign, string project)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            int totalCount = 0;
            int totalPage = 0;
            string whereCondition = " where Week=" + week + " and IsForeign=" + isForeign;
            if (userId != "all")
            {
                whereCondition += " and UserId='" + userId + "' ";
            }
            string whereConditionProject;
            MySqlParameter[] parameters = null;
            if (string.IsNullOrWhiteSpace(project))
            {
                whereConditionProject = whereCondition;
            }
            else
            {
                whereConditionProject = whereCondition + " and ProjectName like @project ";
                parameters = new MySqlParameter[1];
                parameters[0] = GetProjectNameParam(project);
            }
            string limit = " limit " + ((pageIndex - 1) * pageSize).ToString() + ", " + pageSize.ToString();
                
            if (id == "Version")
            {
                List<SummaryVersion> versionList = SqlDbHelper.GetSummaryVersion(whereConditionProject, limit, parameters, out totalCount, out totalPage);
                ViewBag.TotalCount = totalCount;
                ViewBag.TotalPage = totalPage;
                ViewBag.PageIndex = pageIndex;
                return PartialView("_Summary_" + id, versionList);
            }
            else if (id == "Feedback")
            {                
                List<SummaryFeedback> feedbackList = SqlDbHelper.GetSummaryFeedback(whereCondition, limit, null, out totalCount, out totalPage);
                ViewBag.TotalCount = totalCount;
                ViewBag.TotalPage = totalPage;
                ViewBag.PageIndex = pageIndex;
                ViewBag.UserList =  CacheFoxData.UserList;
                return PartialView("_Summary_" + id, feedbackList);
            }
            else if (id == "Suggest")
            {
                List<SummarySuggest> suggestList = SqlDbHelper.GetSummarySuggest(whereCondition, limit, null, out totalCount, out totalPage);
                ViewBag.TotalCount = totalCount;
                ViewBag.TotalPage = totalPage;
                ViewBag.PageIndex = pageIndex;
                ViewBag.UserList = CacheFoxData.UserList;
                return PartialView("_Summary_" + id, suggestList);
            }
            else// "TargetStrategy"
            {
                List<SummaryTargetStrategy> targetList = SqlDbHelper.GetSummaryTargetStrategy(whereConditionProject, limit, parameters, out totalCount, out totalPage);
                ViewBag.TotalCount = totalCount;
                ViewBag.TotalPage = totalPage;
                ViewBag.PageIndex = pageIndex;
                return PartialView("_Summary_" + id, targetList);
            }
        }
        /// <summary>
        /// 三、重点事务，获取每个Tab内容
        /// </summary>
        /// <param name="id">表名称后面部分：Product</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="userId">用户id</param>
        /// <param name="week">年周</param>
        /// <param name="isForeign">0国内，1国外</param>
        /// <returns></returns>
        public PartialViewResult Affair(string id, int pageIndex, string userId, string week, string isForeign)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            int totalCount = 0;
            int totalPage = 0;
            string whereCondition = " where Week=" + week + " and IsForeign=" + isForeign;
            if (userId != "all")
            {
                whereCondition += " and UserId='" + userId + "' ";
            }
            string limit = " limit " + ((pageIndex - 1) * pageSize).ToString() + ", " + pageSize.ToString();

            List<AffairProduct> targetList = SqlDbHelper.GetAffairProduct(whereCondition, limit, null, out totalCount, out totalPage);

            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPage = totalPage;
            ViewBag.PageIndex = pageIndex;
            ViewBag.UserList = CacheFoxData.UserList;

            return PartialView("_Affair_" + id, targetList);
        }
        
         /// <summary>
         /// 二、项目概况，获取分页的项目概况
         /// </summary>
         /// <param name="id">Project_Info表的列名称：Target/Progress/Teamwork/VersionDetail/VersionQuality</param>
         /// <param name="recordId"></param>
         /// <param name="pageIndex"></param>
         /// <param name="userId"></param>
         /// <param name="week"></param>
         /// <param name="isForeign"></param>
         /// <returns></returns>
        public PartialViewResult ProjectInfo(int pageIndex, string userId, string week, string isForeign, string project)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            int totalCount = 0;
            int totalPage = 0;
            string whereCondition = " where Week=" + week + " and IsForeign=" + isForeign;
            if (userId != "all")
            {
                whereCondition += " and UserId='" + userId + "' ";
            }
            MySqlParameter[] parameters = null;
            if (!string.IsNullOrWhiteSpace(project))
            {                
                whereCondition += " and ProjectName like @project ";
                parameters = new MySqlParameter[1];
                parameters[0] = GetProjectNameParam(project);
            }
            string limit = " limit " + ((pageIndex - 1) * pageSize).ToString() + ", " + pageSize.ToString();
            List<ProjectInfo> projectInfoList = SqlDbHelper.GetProjectInfoList(whereCondition, limit, parameters, out totalCount, out totalPage);

            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPage = totalPage;
            ViewBag.PageIndex = pageIndex;
            //ViewBag.UserList = CacheFoxData.UserList;

            return PartialView("_Project_Info", projectInfoList);
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
        public PartialViewResult ProjectInfoTab(string id, int recordId)//, string userId, string week, string isForeign
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
            return PartialView("_Project_Info_Tab", p);
        }

        /// <summary>
        /// 四、团队工作方式优化
        /// </summary>
        /// <param name="id">表名称后面部分：Product</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="userId">用户id</param>
        /// <param name="week">年周</param>
        /// <param name="isForeign">0国内，1国外</param>
        /// <returns></returns>
        public PartialViewResult TeamworkInfo(string id, int pageIndex, string userId, string week, string isForeign)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            int totalCount = 0;
            int totalPage = 0;
            string whereCondition = " where Week=" + week; //+ " and IsForeign=" + isForeign;
            if (userId != "all")
            {
                whereCondition += " and UserId='" + userId + "' ";
            }
            string limit = " limit " + ((pageIndex - 1) * pageSize).ToString() + ", " + pageSize.ToString();

            List<TeamworkInfo> targetList = SqlDbHelper.GetTeamworkInfoList(whereCondition, limit, null, out totalCount, out totalPage);

            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPage = totalPage;
            ViewBag.PageIndex = pageIndex;

            return PartialView("_Teamwork_Info", targetList);
        }

        /// <summary>
        /// 五、需要的协助和支持
        /// </summary>
        /// <param name="id">表名称后面部分：Product</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="userId">用户id</param>
        /// <param name="week">年周</param>
        /// <param name="isForeign">0国内，1国外</param>
        /// <returns></returns>
        public PartialViewResult AssistInfo(string id, int pageIndex, string userId, string week, string isForeign)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            int totalCount = 0;
            int totalPage = 0;
            string whereCondition = " where Week=" + week;   // +" and IsForeign=" + isForeign;
            if (userId != "all")
            {
                whereCondition += " and UserId='" + userId + "' ";
            }
            string limit = " limit " + ((pageIndex - 1) * pageSize).ToString() + ", " + pageSize.ToString();

            List<AssistInfo> targetList = SqlDbHelper.GetAssistInfoList(whereCondition, limit, null, out totalCount, out totalPage);

            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPage = totalPage;
            ViewBag.PageIndex = pageIndex;

            return PartialView("_Assist_Info", targetList);
        }

        /// <summary>
        /// 生成查询Div的html
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchDiv()
        {
            HttpCookie cookie = GetCookie();
            ViewBag.SelectedYearWeek = cookie.Values["week"];
            ViewBag.SelectedUserId = cookie.Values["userId"];
            ViewBag.SelectedIsForeign = cookie.Values["isForeign"];
            ViewBag.YearWeek = DateTime.Now.Year * 100 + WeekHelper.GetWeekOfYear(DateTime.Now);
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
