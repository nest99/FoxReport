using FoxReport.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FoxReport.Helper
{
    public class SqlDbHelper : Loggable<SqlDbHelper>
    {
        private static string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        /// <summary>
        /// 获取初始加载时显示的Model数据
        /// </summary>        
        /// <param name="userId">用户Id</param>
        /// <param name="week">年周</param>
        /// <param name="isForeign">国内=0，国外=1</param>
        /// <returns></returns>
        /*public static InitShow GetInitShow(int userId, int week, int isForeign)
        {
            InitShow initShow = new InitShow();

            MySqlConnection con = new MySqlConnection(ConnectionString);
            string condition = " where UserId=" + userId.ToString() + " and Week=" + week.ToString() + " and IsForeign=" + isForeign.ToString();
            try
            {
                con.Open();
                MySqlDataReader reader = MySqlHelper.ExecuteReader(con, "select * from Summary_TargetStrategy " + condition);
                while (reader.Read())
                {
                    SummaryTargetStrategy t = new SummaryTargetStrategy();
                    t.Id = int.Parse(reader["Id"].ToString());
                    t.ProjectName = reader["ProjectName"].ToString();
                    t.Status = reader["Status"].ToString();
                    t.Strategy = reader["Strategy"].ToString();
                    t.Target = reader["Target"].ToString();
                    t.UserId = reader["UserId"].ToString();
                    t.Week = int.Parse(reader["Week"].ToString());
                    t.IsForeign = int.Parse(reader["IsForeign"].ToString());
                    t.OrderNum = int.Parse(reader["OrderNum"].ToString());
                    initShow.SummaryTargetStrategyList.Add(t);
                }
                reader.Close();

                reader = MySqlHelper.ExecuteReader(con, "select * from Project_Info " + condition);
                while (reader.Read())
                {
                    ProjectInfo p = new ProjectInfo();
                    p.Id = int.Parse(reader["Id"].ToString());
                    p.ProjectName = reader["ProjectName"].ToString();
                    p.Target = reader["Target"].ToString();
                    p.Progress = reader["Progress"].ToString();
                    p.Teamwork = reader["Teamwork"].ToString();
                    p.VersionDetail = reader["VersionDetail"].ToString();
                    p.VersionQuality = reader["VersionQuality"].ToString();
                    p.UserId = reader["UserId"].ToString();
                    p.Week = int.Parse(reader["Week"].ToString());
                    p.IsForeign = int.Parse(reader["IsForeign"].ToString());
                    p.OrderNum = int.Parse(reader["OrderNum"].ToString());                    
                    initShow.ProjectInfoList.Add(p);
                }
                reader.Close();

                reader = MySqlHelper.ExecuteReader(con, "select * from Affair_Product " + condition);
                while (reader.Read())
                {
                    AffairProduct p = new AffairProduct();
                    p.Id = int.Parse(reader["Id"].ToString());
                    p.Classify = reader["Classify"].ToString();
                    p.Priority = reader["Priority"].ToString();
                    p.Progress = reader["Progress"].ToString();
                    p.Tracker = reader["Tracker"].ToString();
                    p.Workplan = reader["Workplan"].ToString();
                    p.UserId = reader["UserId"].ToString();
                    p.Week = int.Parse(reader["Week"].ToString());
                    p.IsForeign = int.Parse(reader["IsForeign"].ToString());
                    p.OrderNum = int.Parse(reader["OrderNum"].ToString());
                    initShow.AffairProductList.Add(p);
                }
                reader.Close();

                reader = MySqlHelper.ExecuteReader(con, "select * from Teamwork_Info " + condition);
                while (reader.Read())
                {
                    initShow.teamworkInfo.Id = int.Parse(reader["Id"].ToString());
                    initShow.teamworkInfo.Content = reader["Content"].ToString();
                    initShow.teamworkInfo.UserId = reader["UserId"].ToString();
                    initShow.teamworkInfo.Week = int.Parse(reader["Week"].ToString());
                    initShow.teamworkInfo.IsForeign = int.Parse(reader["IsForeign"].ToString());
                    initShow.teamworkInfo.OrderNum = int.Parse(reader["OrderNum"].ToString());
                }
                reader.Close();

                reader = MySqlHelper.ExecuteReader(con, "select * from Assist_Info " + condition);
                while (reader.Read())
                {
                    AssistInfo p = new AssistInfo();
                    initShow.assistInfo.Id = int.Parse(reader["Id"].ToString());
                    initShow.assistInfo.Content = reader["Content"].ToString();
                    initShow.assistInfo.UserId = reader["UserId"].ToString();
                    initShow.assistInfo.Week = int.Parse(reader["Week"].ToString());
                    initShow.assistInfo.IsForeign = int.Parse(reader["IsForeign"].ToString());
                    initShow.assistInfo.OrderNum = int.Parse(reader["OrderNum"].ToString());
                }
                reader.Close();
            }
            catch (MySqlException e)
            {
                Logger.Error("查询InitShow出错。", e);
            }
            finally
            {
                con.Close();
            }

            return initShow;
        }

        public static InitReport GetInitReport(string whereCondition, string limit, MySqlParameter[] parameters, out int[] totalCount, out int[] totalPage)
        {
            totalCount = new int[3];
            totalPage = new int[3];

            InitReport initReport = new InitReport();
            initReport.ReportName = GetReportName(whereCondition);
            initReport.SummaryTargetStrategyList = GetSummaryTargetStrategy(whereCondition, limit, parameters, out totalCount[0], out totalPage[0]);
            initReport.ProjectInfoList = GetProjectInfoList(whereCondition, limit, out totalCount[1], out totalPage[1]);
            initReport.AffairProductList = GetAffairProduct(whereCondition, limit, out totalCount[2], out totalPage[2]);
            initReport.teamworkInfo = GetTeamworkInfo(whereCondition);
            initReport.assistInfo = GetAssistInfo(whereCondition);
            return initReport;
        }
        */
        
        public static int DeleteData(string tableName, string id)
        {
            string sql = "delete from " + tableName + " where Id=" + id.ToString();
            MySqlConnection con = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand(sql, con);        

            int deleteCount = 0;
            try
            {
                con.Open();
                deleteCount = cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Logger.Error("删除数据出错，sql=" + sql, e);
            }
            finally
            {
                con.Close();
            }
            return deleteCount;
        }
        /// <summary>
        /// 保存富文本字段值到数据库
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">字段名称</param>
        /// <param name="columnValue">字段值</param>
        /// <param name="id">行id</param>
        /// <returns></returns>
        public static int SaveText(string tableName, string columnName, string columnValue, string id, string userId, string week, string isForeign)
        {
            string sql = "";
            if (id == "0")
            {
                sql = "insert into " + tableName + " (" + columnName + ", UserId, Week, IsForeign) " +
                    " values(@value, '" + userId + "', " + week + ", " + isForeign + ");";
            }
            else
            {
                sql = "update " + tableName + " set " + columnName + "=@value" + " where id=@id ";
            }

            MySqlConnection con = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.Parameters.Add("@value", MySqlDbType.VarString).Value = columnValue;
            cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

            int newId = 0;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                if (id == "0")
                {
                    newId = (int)cmd.LastInsertedId;
                }
            }
            catch (MySqlException e)
            {
                Logger.Error("SaveText执行sql出错，sql=" + sql + ", value=" + columnValue + ", id=" + id, e);
            }
            finally
            {
                con.Close();
            }
            return newId;
        }
        /// <summary>
        /// 保存纯文本字段值到数据库
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">字段名称</param>
        /// <param name="columnValue">字段值</param>
        /// <param name="id">行id</param>
        public static int SaveColumnText(string tableName, string columnName, string columnValue, string id, string userId, string week, string isForeign)
        {
            string sql = "";
            if (id == "0")
            {
                sql = "insert into " + tableName + " (" + columnName + ", UserId, Week, IsForeign) " +
                    " values(@value, '" + userId + "', " + week + ", " + isForeign + ");";
            }
            else
            {
                sql = "update " + tableName + " set " + columnName + "=@value" + " where id=@id ";
            }

            MySqlConnection con = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.Parameters.Add("@value", MySqlDbType.VarString).Value = columnValue;
            cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

            int newId = 0;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                if (id == "0")
                {
                    newId = (int)cmd.LastInsertedId;
                }
            }
            catch (MySqlException e)
            {
                Logger.Error("SaveColumnText执行sql出错，sql=" + sql + ", value=" + columnValue + ", id=" + id, e);
            }
            finally
            {
                con.Close();
            }
            return newId;
        }
        public static string GetReportName(string whereCondition)
        {
            string sql = "select ReportName from report_info " + whereCondition;
            MySqlConnection con = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand(sql, con);
            string reportName = "";
            try
            {
                con.Open();                
                object name = cmd.ExecuteScalar();
                reportName = name == null ? "" : name.ToString();
            }
            catch (MySqlException e)
            {
                Logger.Error("执行sql出错，sql=" + sql, e);
            }
            finally
            {
                con.Close();
            }
            return reportName;
        }
        public static int SaveReportName(string reportName, string userId, string week, string isForeign)
        {
            string sqlExist = "select count(*) from report_info where UserId = '" + userId + "' and Week = " + week + " and IsForeign=" + isForeign;
            string sql = "";
            MySqlConnection con = new MySqlConnection(ConnectionString);
            MySqlCommand cmdExist = new MySqlCommand(sqlExist, con);            
            
            int result = 0;
            try
            {
                con.Open();
                int count = int.Parse(cmdExist.ExecuteScalar().ToString());                
                if (count == 0)
                {
                    sql = "insert report_info(ReportName, userid, week, isforeign) values(@ReportName, '" + userId + "', " + week + ", " + isForeign + ")";
                }
                else
                {
                    sql = "update report_info set ReportName = @ReportName where  userId = '" + userId + "' and Week = " + week + " and IsForeign=" + isForeign;
                }
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.Add("@ReportName", MySqlDbType.VarString, 50).Value = reportName;
                result = cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Logger.Error("执行sql出错，reprotName = " + reportName + ", sqlExist=" + sqlExist + ", sql=" + sql, e);
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        /// <summary>
        /// 插入、更新Seq序号
        /// </summary>
        /// <param name="seqTable">哪个表：Feedback/Suggest</param>
        /// <param name="recordId">记录id</param>
        /// <param name="seq">序号</param>
        /// <returns></returns>
        public static int SaveSeq(string seqTable, string recordId, string seq)
        {            
            string sql = "";
            if (recordId == "0")
            {
                sql = "insert into Summary_" + seqTable + "(Seq) values(" + seq + ")";
            }
            else
            {
                sql = "update Summary_" + seqTable + " set Seq=" + seq + " where Id=" + recordId;
            }
            MySqlConnection con = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand(sql, con);

            int newId = 0;
            try
            {
                con.Open();
                newId = cmd.ExecuteNonQuery();
                if (recordId == "0")
                {
                    newId = (int)cmd.LastInsertedId;
                }
            }
            catch (MySqlException e)
            {
                Logger.Error("SaveSeq执行sql出错，sql=" + sql, e);
            }
            finally
            {
                con.Close();
            }
            return newId;
        }
        /// <summary>
        /// 一、整体概况
        /// </summary>
        public static List<SummaryTargetStrategy> GetSummaryTargetStrategy(string whereCondition, string limit, MySqlParameter[] parameters, out int totalCount, out int totalPage)
        {
            List<SummaryTargetStrategy> targetList = new List<SummaryTargetStrategy>();
            MySqlConnection con = new MySqlConnection(ConnectionString);
            totalCount = 0;
            try
            {
                con.Open();
                totalCount = int.Parse(MySqlHelper.ExecuteScalar(con, "select count(*) from Summary_TargetStrategy " + whereCondition, parameters).ToString());
                MySqlDataReader reader = MySqlHelper.ExecuteReader(con, "select * from Summary_TargetStrategy " + whereCondition + limit, parameters);
                while (reader.Read())
                {
                    SummaryTargetStrategy t = new SummaryTargetStrategy();
                    t.Id = int.Parse(reader["Id"].ToString());
                    t.ProjectName = reader["ProjectName"].ToString();
                    t.Status = reader["Status"].ToString();
                    t.Strategy = reader["Strategy"].ToString();
                    t.Target = reader["Target"].ToString();
                    t.UserId = reader["UserId"].ToString();
                    t.Week = int.Parse(reader["Week"].ToString());
                    t.IsForeign = int.Parse(reader["IsForeign"].ToString());
                    t.OrderNum = int.Parse(reader["OrderNum"].ToString());
                    targetList.Add(t);
                }
                reader.Close();
            }
            catch (MySqlException e)
            {
                Logger.Error("查询SummaryTargetStrategy出错。", e);
            }
            finally
            {
                con.Close();
            }
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            totalPage = totalCount % pageSize == 0 ? totalCount / pageSize : totalCount / pageSize + 1;

            return targetList;
        }

        public static List<SummaryVersion> GetSummaryVersion(string whereCondition, string limit, MySqlParameter[] parameters, out int totalCount, out int totalPage)
        {
            List<SummaryVersion> versionList = new List<SummaryVersion>();
            MySqlConnection con = new MySqlConnection(ConnectionString);
            totalCount = 0;
            try
            {
                con.Open();
                totalCount = int.Parse(MySqlHelper.ExecuteScalar(con, "select count(*) from Summary_Version " + whereCondition, parameters).ToString());
                MySqlDataReader reader = MySqlHelper.ExecuteReader(con, "select * from Summary_Version " + whereCondition + limit, parameters);
                while (reader.Read())
                {
                    SummaryVersion v = new SummaryVersion();
                    v.Id = int.Parse(reader["Id"].ToString());
                    v.ProjectName = reader["ProjectName"].ToString();
                    v.Request = reader["Request"].ToString();
                    v.Publish = reader["Publish"].ToString();
                    v.Risk = reader["Risk"].ToString();
                    v.UserId = reader["UserId"].ToString();
                    v.Week = int.Parse(reader["Week"].ToString());
                    v.IsForeign = int.Parse(reader["IsForeign"].ToString());
                    v.OrderNum = int.Parse(reader["OrderNum"].ToString());
                    versionList.Add(v);
                }
                reader.Close();
            }
            catch (MySqlException e)
            {
                Logger.Error("查询SummaryVersion出错。", e);
            }
            finally
            {
                con.Close();
            }
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            totalPage = totalCount % pageSize == 0 ? totalCount / pageSize : totalCount / pageSize + 1;

            return versionList;
        }

        public static List<SummaryFeedback> GetSummaryFeedback(string whereCondition, string limit, MySqlParameter[] parameters, out int totalCount, out int totalPage)
        {
            List<SummaryFeedback> feedbackList = new List<SummaryFeedback>();
            MySqlConnection con = new MySqlConnection(ConnectionString);
            totalCount = 0;
            try
            {
                con.Open();
                totalCount = int.Parse(MySqlHelper.ExecuteScalar(con, "select count(*) from Summary_Feedback " + whereCondition, parameters).ToString());
                MySqlDataReader reader = MySqlHelper.ExecuteReader(con, "select * from Summary_Feedback " + whereCondition + limit, parameters);
                while (reader.Read())
                {
                    SummaryFeedback f = new SummaryFeedback();
                    f.Id = int.Parse(reader["Id"].ToString());
                    f.Seq = reader["Seq"].ToString();
                    f.Platform = reader["Platform"].ToString();
                    f.Issue = reader["Issue"].ToString();
                    f.Tracker = reader["Tracker"].ToString();
                    f.TrackInfo = reader["TrackInfo"].ToString();
                    f.Status = reader["Status"].ToString();
                    f.UserId = reader["UserId"].ToString();
                    f.Week = int.Parse(reader["Week"].ToString());
                    f.IsForeign = int.Parse(reader["IsForeign"].ToString());
                    f.OrderNum = int.Parse(reader["OrderNum"].ToString());
                    feedbackList.Add(f);
                }
                reader.Close();
            }
            catch (MySqlException e)
            {
                Logger.Error("查询SummaryFeedback出错。", e);
            }
            finally
            {
                con.Close();
            }
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            totalPage = totalCount % pageSize == 0 ? totalCount / pageSize : totalCount / pageSize + 1;

            return feedbackList;
        }

        public static List<SummarySuggest> GetSummarySuggest(string whereCondition, string limit, MySqlParameter[] parameters, out int totalCount, out int totalPage)
        {
            List<SummarySuggest> suggestList = new List<SummarySuggest>();
            MySqlConnection con = new MySqlConnection(ConnectionString);
            totalCount = 0;
            try
            {
                con.Open();
                totalCount = int.Parse(MySqlHelper.ExecuteScalar(con, "select count(*) from Summary_Suggest " + whereCondition, parameters).ToString());
                MySqlDataReader reader = MySqlHelper.ExecuteReader(con, "select * from Summary_Suggest " + whereCondition + limit, parameters);
                while (reader.Read())
                {
                    SummarySuggest s = new SummarySuggest();
                    s.Id = int.Parse(reader["Id"].ToString());
                    s.Seq = reader["Seq"].ToString();
                    s.Platform = reader["Platform"].ToString();
                    s.Issue = reader["Issue"].ToString();
                    s.SuggestContent = reader["SuggestContent"].ToString();
                    s.TrackInfo = reader["TrackInfo"].ToString();
                    s.UserCount = reader["UserCount"].ToString();
                    s.UserId = reader["UserId"].ToString();
                    s.Week = int.Parse(reader["Week"].ToString());
                    s.IsForeign = int.Parse(reader["IsForeign"].ToString());
                    s.OrderNum = int.Parse(reader["OrderNum"].ToString());
                    suggestList.Add(s);
                }
                reader.Close();
            }
            catch (MySqlException e)
            {
                Logger.Error("查询SummarySuggest出错。", e);
            }
            finally
            {
                con.Close();
            }
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            totalPage = totalCount % pageSize == 0 ? totalCount / pageSize : totalCount / pageSize + 1;

            return suggestList;
        }

        /// <summary>
        /// 二、项目概况
        /// </summary>
        public static ProjectInfo GetProjectInfo(string whereCondition)
        {
            ProjectInfo projectInfo = new ProjectInfo();
            MySqlConnection con = new MySqlConnection(ConnectionString);
            try
            {
                con.Open();               
                MySqlDataReader reader = MySqlHelper.ExecuteReader(con, "select * from Project_Info " + whereCondition);
                while (reader.Read())
                {
                    projectInfo.Id = int.Parse(reader["Id"].ToString());
                    projectInfo.ProjectName = reader["ProjectName"].ToString();
                    projectInfo.Target = reader["Target"].ToString();
                    projectInfo.Progress = reader["Progress"].ToString();
                    projectInfo.Teamwork = reader["Teamwork"].ToString();
                    projectInfo.VersionDetail = reader["VersionDetail"].ToString();
                    projectInfo.VersionQuality = reader["VersionQuality"].ToString();
                    projectInfo.UserId = reader["UserId"].ToString();
                    projectInfo.Week = int.Parse(reader["Week"].ToString());
                    projectInfo.IsForeign = int.Parse(reader["IsForeign"].ToString());
                    projectInfo.OrderNum = int.Parse(reader["OrderNum"].ToString());                    
                }
                reader.Close();
            }
            catch (MySqlException e)
            {
                Logger.Error("查询ProjectInfo出错。", e);
            }
            finally
            {
                con.Close();
            }
            
            return projectInfo;
        }
        /// <summary>
        /// 二、项目概况列表
        /// </summary>
        public static List<ProjectInfo> GetProjectInfoList(string whereCondition, string limit, MySqlParameter[] parameters, out int totalCount, out int totalPage)
        {
            List<ProjectInfo> projectInfoList = new List<ProjectInfo>();
            MySqlConnection con = new MySqlConnection(ConnectionString);
            totalCount = 0;
            try
            {
                con.Open();
                totalCount = int.Parse(MySqlHelper.ExecuteScalar(con, "select count(*) from Project_Info " + whereCondition, parameters).ToString());
                MySqlDataReader reader = MySqlHelper.ExecuteReader(con, "select * from Project_Info " + whereCondition + limit, parameters);
                while (reader.Read())
                {
                    ProjectInfo p = new ProjectInfo();
                    p.Id = int.Parse(reader["Id"].ToString());
                    p.ProjectName = reader["ProjectName"].ToString();
                    p.Target = reader["Target"].ToString();
                    p.Progress = reader["Progress"].ToString();
                    p.Teamwork = reader["Teamwork"].ToString();
                    p.VersionDetail = reader["VersionDetail"].ToString();
                    p.VersionQuality = reader["VersionQuality"].ToString();
                    p.UserId = reader["UserId"].ToString();
                    p.Week = int.Parse(reader["Week"].ToString());
                    p.IsForeign = int.Parse(reader["IsForeign"].ToString());
                    p.OrderNum = int.Parse(reader["OrderNum"].ToString());
                    projectInfoList.Add(p);
                }
                reader.Close();
            }
            catch (MySqlException e)
            {
                Logger.Error("查询ProjectInfoList出错。", e);
            }
            finally
            {
                con.Close();
            }
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            totalPage = totalCount % pageSize == 0 ? totalCount / pageSize : totalCount / pageSize + 1;

            return projectInfoList;
        }
        /// <summary>
        /// 三、重点事务：产品事务
        /// </summary>
        public static List<AffairProduct> GetAffairProduct(string whereCondition, string limit, MySqlParameter[] parameters, out int totalCount, out int totalPage)
        {
            List<AffairProduct> productList = new List<AffairProduct>();
            MySqlConnection con = new MySqlConnection(ConnectionString);
            totalCount = 0;
            try
            {
                con.Open();
                totalCount = int.Parse(MySqlHelper.ExecuteScalar(con, "select count(*) from Affair_Product " + whereCondition, parameters).ToString());
                MySqlDataReader reader = MySqlHelper.ExecuteReader(con, "select * from Affair_Product " + whereCondition + limit, parameters);
                while (reader.Read())
                {
                    AffairProduct p = new AffairProduct();
                    p.Id = int.Parse(reader["Id"].ToString());
                    p.Classify = reader["Classify"].ToString();
                    p.Priority = reader["Priority"].ToString();
                    p.Progress = reader["Progress"].ToString();
                    p.Tracker = Helper.SqlDbHelper.GetUserName(reader["Tracker"].ToString());
                    p.Workplan = reader["Workplan"].ToString();
                    p.UserId = reader["UserId"].ToString();
                    p.Week = int.Parse(reader["Week"].ToString());
                    p.IsForeign = int.Parse(reader["IsForeign"].ToString());
                    p.OrderNum = int.Parse(reader["OrderNum"].ToString());
                    productList.Add(p);
                }
                reader.Close();
            }
            catch (MySqlException e)
            {
                Logger.Error("查询InitShow出错。", e);
            }
            finally
            {
                con.Close();
            }
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            totalPage = totalCount % pageSize == 0 ? totalCount / pageSize : totalCount / pageSize + 1;

            return productList;
        }
        
        /// <summary>
        /// 四、团队工作方式优化
        /// </summary>
        public static List<TeamworkInfo> GetTeamworkInfoList(string whereCondition, string limit, MySqlParameter[] parameters, out int totalCount, out int totalPage)
        {
            List<TeamworkInfo> teamworkInfoList = new List<TeamworkInfo>();
            MySqlConnection con = new MySqlConnection(ConnectionString);
            totalCount = 0;
            try
            {
                con.Open();
                totalCount = int.Parse(MySqlHelper.ExecuteScalar(con, "select count(*) from Teamwork_Info " + whereCondition, parameters).ToString());
                MySqlDataReader reader = MySqlHelper.ExecuteReader(con, "select * from Teamwork_Info " + whereCondition + limit, parameters);                
                while (reader.Read())
                {
                    TeamworkInfo teamworkInfo = new TeamworkInfo();
                    teamworkInfo.Id = int.Parse(reader["Id"].ToString());
                    teamworkInfo.Content = reader["Content"].ToString();
                    teamworkInfo.UserId = reader["UserId"].ToString();
                    teamworkInfo.Week = int.Parse(reader["Week"].ToString());
                    teamworkInfo.IsForeign = int.Parse(reader["IsForeign"].ToString());
                    teamworkInfo.OrderNum = int.Parse(reader["OrderNum"].ToString());
                    teamworkInfoList.Add(teamworkInfo);
                }
                reader.Close();
            }
            catch (MySqlException e)
            {
                Logger.Error("查询Teamwork_Info出错。", e);
            }
            finally
            {
                con.Close();
            }
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            totalPage = totalCount % pageSize == 0 ? totalCount / pageSize : totalCount / pageSize + 1;

            return teamworkInfoList;
        }
        /// <summary>
        /// 五、需要的协助和支持
        /// </summary>
        public static List<AssistInfo> GetAssistInfoList(string whereCondition, string limit, MySqlParameter[] parameters, out int totalCount, out int totalPage)
        {
            List<AssistInfo> assistInfoList = new List<AssistInfo>();
            MySqlConnection con = new MySqlConnection(ConnectionString);
            totalCount = 0;
            try
            {
                con.Open();
                totalCount = int.Parse(MySqlHelper.ExecuteScalar(con, "select count(*) from Assist_Info " + whereCondition, parameters).ToString());
                MySqlDataReader reader = MySqlHelper.ExecuteReader(con, "select * from Assist_Info " + whereCondition + limit, parameters);                
                
                while (reader.Read())
                {
                    AssistInfo assistInfo = new AssistInfo();
                    assistInfo.Id = int.Parse(reader["Id"].ToString());
                    assistInfo.Content = reader["Content"].ToString();
                    assistInfo.UserId = reader["UserId"].ToString();
                    assistInfo.Week = int.Parse(reader["Week"].ToString());
                    assistInfo.IsForeign = int.Parse(reader["IsForeign"].ToString());
                    assistInfo.OrderNum = int.Parse(reader["OrderNum"].ToString());
                    assistInfoList.Add(assistInfo);
                }
                reader.Close();
            }
            catch (MySqlException e)
            {
                Logger.Error("查询AssistInfo出错。", e);
            }
            finally
            {
                con.Close();
            }
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            totalPage = totalCount % pageSize == 0 ? totalCount / pageSize : totalCount / pageSize + 1;

            return assistInfoList;
        }
        #region 复制周报数据
        /// <summary>
        /// 复制周报数据
        /// </summary>
        /// <param name="reportUserId">新周报填写人</param>
        /// <param name="yearWeek">新周报年周</param>
        /// <param name="condition">复制条件</param>
        /// <returns></returns>
        public static bool CopyWeekReport(string reportUserId, string yearWeek, string condition, string projectName)
        {
            bool success = true;
            string conditionProject;
            MySqlParameter parameter = null;
            if (string.IsNullOrWhiteSpace(projectName))
            {
                conditionProject = condition;
            }
            else
            {
                conditionProject = condition + " and ProjectName like @project ";
                parameter = new MySqlParameter("@project", MySqlDbType.VarString, 50);
                parameter.Value = "%" + projectName + "%";
            }

            CopyTableAffairProduct(reportUserId, yearWeek, condition);
            CopyTableAssistInfo(reportUserId, yearWeek, condition);
            CopyTableProjectInfo(reportUserId, yearWeek, conditionProject, parameter);
            CopyTableSummaryFeedback(reportUserId, yearWeek, condition);
            CopyTableSummarySuggest(reportUserId, yearWeek, condition);
            CopyTableSummaryTargetStrategy(reportUserId, yearWeek, conditionProject, parameter);
            CopyTableSummaryVersion(reportUserId, yearWeek, conditionProject, parameter);
            CopyTableTeamworkInfo(reportUserId, yearWeek, condition);

            return success;
        }
       
        public static bool CopyTableSummaryTargetStrategy(string reportUserId, string yearWeek, string conditionProject, MySqlParameter parameter)
        {
            bool success = true;
            string sql = "INSERT INTO `summary_targetstrategy` (`ProjectName`, `Status`, `Target`, `Strategy`, `UserId`, `Week`, `IsForeign`, `OrderNum`)  " +
                               " SELECT `ProjectName`, `Status`, `Target`, `Strategy`, '" + reportUserId + "', " + yearWeek + ", `IsForeign`, `OrderNum` FROM `summary_targetstrategy` where " + conditionProject;
            MySqlConnection con = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand(sql, con);
            if (parameter != null)
            {
                cmd.Parameters.Add(parameter);
            }

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                success = false;
                Logger.Error("复制summary_targetstrategy出错。condition=" + conditionProject, e);
            }
            finally
            {
                con.Close();
            }
            return success;
        }
        public static bool CopyTableSummaryVersion(string reportUserId, string yearWeek, string conditionProject, MySqlParameter parameter)
        {
            bool success = true;
            string sql = "INSERT INTO `summary_version` (`ProjectName`, `Request`, `Publish`, `Risk`, `UserId`, `Week`, `IsForeign`, `OrderNum`)  " +
                               " SELECT `ProjectName`, `Request`, `Publish`, `Risk`, '" + reportUserId + "', " + yearWeek + ", `IsForeign`, `OrderNum` FROM `summary_version` where " + conditionProject;
            MySqlConnection con = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand(sql, con);
            if (parameter != null)
            {
                cmd.Parameters.Add(parameter);
            }

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                success = false;
                Logger.Error("复制summary_version出错。condition=" + conditionProject, e);
            }
            finally
            {
                con.Close();
            }
            return success;
        }
        public static bool CopyTableProjectInfo(string reportUserId, string yearWeek, string conditionProject, MySqlParameter parameter)
        {
            bool success = true;
            string sql = "INSERT INTO `project_info` (`ProjectName`, `Target`, `Progress`, `Teamwork`, `VersionDetail`, `VersionQuality`, `UserId`, `Week`, `IsForeign`, `OrderNum`)  " +
                               " SELECT `ProjectName`, `Target`, `Progress`, `Teamwork`, `VersionDetail`, `VersionQuality`, '" + reportUserId + "', " + yearWeek + ", `IsForeign`, `OrderNum` FROM `project_info` where " + conditionProject;
            MySqlConnection con = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand(sql, con);
            if (parameter != null)
            {
                cmd.Parameters.Add(parameter);
            }
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                success = false;
                Logger.Error("复制project_info出错。condition=" + conditionProject, e);
            }
            finally
            {
                con.Close();
            }
            return success;
        }

        public static bool CopyTableSummaryFeedback(string reportUserId, string yearWeek, string condition)
        {
            bool success = true;
            string sql = "INSERT INTO `summary_feedback` (`Seq`, `Platform`, `Issue`, `Tracker`, `Status`, `TrackInfo`, `UserId`, `Week`, `IsForeign`, `OrderNum`)  " +
                               " SELECT `Seq`, `Platform`, `Issue`, `Tracker`, `Status`, `TrackInfo`, '" + reportUserId + "', " + yearWeek + ", `IsForeign`, `OrderNum` FROM `summary_feedback` where " + condition;
            MySqlConnection con = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                success = false;
                Logger.Error("复制summary_feedback出错。condition=" + condition, e);
            }
            finally
            {
                con.Close();
            }
            return success;
        }
        public static bool CopyTableSummarySuggest(string reportUserId, string yearWeek, string condition)
        {
            bool success = true;
            string sql = "INSERT INTO `summary_suggest` (`Seq`, `Platform`, `SuggestContent`, `UserCount`, `Issue`, `TrackInfo`, `UserId`, `Week`, `IsForeign`, `OrderNum`)  " +
                               " SELECT `Seq`, `Platform`, `SuggestContent`, `UserCount`, `Issue`, `TrackInfo`, '" + reportUserId + "', " + yearWeek + ", `IsForeign`, `OrderNum` FROM `summary_suggest` where " + condition;
            MySqlConnection con = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                success = false;
                Logger.Error("复制summary_suggest出错。condition=" + condition, e);
            }
            finally
            {
                con.Close();
            }
            return success;
        }
        public static bool CopyTableAffairProduct(string reportUserId, string yearWeek, string condition)
        {
            bool success = true;            
            string sql = "INSERT INTO  `affair_product` (`Classify`, `Priority`, `Tracker`, `Workplan`, `Progress`, `UserId`, `Week`, `IsForeign`, `OrderNum`)  " +
                               " SELECT `Classify`, `Priority`, `Tracker`, `Workplan`, `Progress`, '" + reportUserId + "', " + yearWeek + ", `IsForeign`, `OrderNum` FROM `affair_product` where " + condition;
            MySqlConnection con = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                success = false;
                Logger.Error("复制affair_product出错。condition=" + condition, e);
            }
            finally
            {
                con.Close();
            }
            return success;
        }
        public static bool CopyTableAssistInfo(string reportUserId, string yearWeek, string condition)
        {
            bool success = true;            
            string sql = "INSERT INTO `assist_info` (`Content`, `UserId`, `Week`, `IsForeign`, `OrderNum`)  " +
                               " SELECT `Content`, '" + reportUserId + "', " + yearWeek + ", `IsForeign`, `OrderNum` FROM `assist_info` where " + condition;
            MySqlConnection con = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                success = false;
                Logger.Error("复制assist_info出错。condition=" + condition, e);
            }
            finally
            {
                con.Close();
            }
            return success;
        }
        public static bool CopyTableTeamworkInfo(string reportUserId, string yearWeek, string condition)
        {
            bool success = true;
            string sql = "INSERT INTO `teamwork_info` (`Content`, `UserId`, `Week`, `IsForeign`, `OrderNum`)  " +
                               " SELECT `Content`, '" + reportUserId + "', " + yearWeek + ", `IsForeign`, `OrderNum` FROM `teamwork_info` where " + condition;
            MySqlConnection con = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                success = false;
                Logger.Error("复制teamwork_info出错。condition=" + condition, e);
            }
            finally
            {
                con.Close();
            }
            return success;
        }
        #endregion
        /// <summary>
        /// 获取所有用户的信息
        /// </summary>
        /// <returns></returns>
        public static List<UserInfo> GetUsersInfo()
        {
            List<UserInfo> userInfoList = new List<UserInfo>();
            MySqlConnection con = new MySqlConnection(ConnectionString);
            try
            {
                con.Open();
                MySqlDataReader reader = MySqlHelper.ExecuteReader(con, "select * from UserInfo ");
                while (reader.Read())
                {
                    UserInfo u = new UserInfo();                   
                    u.UserName = reader["UserName"].ToString();
                    u.UserRole = int.Parse(reader["UserRole"].ToString());                   
                    u.UserId = reader["UserId"].ToString();

                    userInfoList.Add(u);
                }
                reader.Close();
            }
            catch (MySqlException e)
            {
                Logger.Error("查询UserInfo出错。", e);
            }
            finally
            {
                con.Close();
            }

            return userInfoList;
        }
        public static string AddUser(string userName, string userRole)
        {
            string sql = "insert into UserInfo(UserName, UserRole) values(@UserName, @UserRole)";
            MySqlConnection con = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.Parameters.Add("@UserName", MySqlDbType.VarString, 50).Value = userName.Trim();
            cmd.Parameters.Add("@UserRole", MySqlDbType.Int32).Value = userRole;
            long result = 0;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                result = cmd.LastInsertedId;
            }
            catch (MySqlException e)
            {
                Logger.Error("新增UserInfo出错。 userName=" + userName, e);
            }
            finally
            {
                con.Close();
            }
            if (result > 0)
            {
                CacheFoxData.RefreshUserInfo();
            }
            return result.ToString();
        }
        public static bool EditUser(string userId, string userName)
        {
            string sql = "update UserInfo set UserName=@UserName where UserId=@UserId";
            MySqlConnection con = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.Parameters.Add("@UserName", MySqlDbType.VarString, 50).Value = userName.Trim();
            cmd.Parameters.Add("@UserId", MySqlDbType.Int32).Value = userId;
            int result = 0;
            try
            {
                con.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Logger.Error("修改UserInfo出错。userId=" + userId + ", userName=" + userName, e);
            }
            finally
            {
                con.Close();
            }
            if (result > 0)
            {
                CacheFoxData.RefreshUserInfo();
            }
            return result > 0;
        }


        public static string GetUserName(string sID)
        {
            string sql = "select UserName from UserInfo where UserID='" + sID + "'";
            MySqlConnection con = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand(sql, con);
            string UserName = "";
            try
            {
                con.Open();
                object name = cmd.ExecuteScalar();
                UserName = name == null ? "" : name.ToString();
            }
            catch (MySqlException e)
            {
                Logger.Error("执行sql出错，sql=" + sql, e);
            }
            finally
            {
                con.Close();
            }
            return UserName;
        }
        public static bool DeleteUser(string userId)
        {
            string sql = "delete from UserInfo where UserId=@UserId";
            MySqlConnection con = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand(sql, con);            
            cmd.Parameters.Add("@UserId", MySqlDbType.Int32).Value = userId;
            int result = 0;
            try
            {
                con.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Logger.Error("删除UserInfo出错。userId=" + userId , e);
            }
            finally
            {
                con.Close();
            }
            if (result > 0)
            {
                CacheFoxData.RefreshUserInfo();
            }
            return result > 0;
        }
        /// <summary>
        /// 获取所有周的信息
        /// </summary>
        /// <returns></returns>
        public static List<WeekInfo> GetWeekInfo()
        {
            List<WeekInfo> weekInfoList = new List<WeekInfo>();
            MySqlConnection con = new MySqlConnection(ConnectionString);
            
            try
            {
                con.Open();
                MySqlDataReader reader = MySqlHelper.ExecuteReader(con, "select * from WeekStartEndDay ");
                while (reader.Read())
                {
                    WeekInfo w = new WeekInfo();
                    w.Year = int.Parse(reader["YearNum"].ToString());
                    w.Week = int.Parse(reader["WeekNum"].ToString());
                    w.YearWeek = int.Parse(reader["YearWeek"].ToString());
                    w.WeekEnd = DateTime.Parse(reader["WeekEnd"].ToString());
                    w.WeekStart = DateTime.Parse(reader["WeekStart"].ToString());
                    weekInfoList.Add(w);
                }
                reader.Close();
            }
            catch (MySqlException e)
            {
                Logger.Error("查询WeekInfo出错。", e);
            }
            finally
            {
                con.Close();
            }

            return weekInfoList;
        }

    }
}