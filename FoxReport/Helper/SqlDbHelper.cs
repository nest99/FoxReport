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
        public static InitShow GetInitShow(int userId, int week, int isForeign)
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
        public static int SaveText(string tableName, string columnName, string columnValue, string id)
        {
            string sql = "";
            if (id == "0")
            {
                sql = "insert into " + tableName + " (" + columnName + ") values(@value);";
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
                Logger.Error("执行sql出错，sql=" + sql + ", value=" + columnValue + ", id=" + id, e);
            }
            finally
            {
                con.Close();
            }
            return newId;
        }

        public static List<SummaryTargetStrategy> GetSummaryTargetStrategy(int userId, int week, int isForeign)
        {
            List<SummaryTargetStrategy> targetList = new List<SummaryTargetStrategy>();
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

            return targetList;
        }

        public static List<SummaryVersion> GetSummaryVersion(int userId, int week, int isForeign)
        {
            List<SummaryVersion> versionList = new List<SummaryVersion>();
            MySqlConnection con = new MySqlConnection(ConnectionString);
            string condition = " where UserId=" + userId.ToString() + " and Week=" + week.ToString() + " and IsForeign=" + isForeign.ToString();
            try
            {
                con.Open();
                MySqlDataReader reader = MySqlHelper.ExecuteReader(con, "select * from Summary_Version " + condition);
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

            return versionList;
        }

        public static List<SummaryFeedback> GetSummaryFeedback(int userId, int week, int isForeign)
        {
            List<SummaryFeedback> feedbackList = new List<SummaryFeedback>();
            MySqlConnection con = new MySqlConnection(ConnectionString);
            string condition = " where UserId=" + userId.ToString() + " and Week=" + week.ToString() + " and IsForeign=" + isForeign.ToString();
            try
            {
                con.Open();
                MySqlDataReader reader = MySqlHelper.ExecuteReader(con, "select * from Summary_Feedback " + condition);
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

            return feedbackList;
        }

        public static List<ProjectInfo> GetProjectInfo(int userId, int week, int isForeign)
        {
            List<ProjectInfo> projectInfoList = new List<ProjectInfo>();
            MySqlConnection con = new MySqlConnection(ConnectionString);
            string condition = " where UserId=" + userId.ToString() + " and Week=" + week.ToString() + " and IsForeign=" + isForeign.ToString();
            try
            {
                con.Open();
                MySqlDataReader reader = MySqlHelper.ExecuteReader(con, "select * from Project_Info " + condition);
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
                Logger.Error("查询ProjectInfo出错。", e);
            }
            finally
            {
                con.Close();
            }

            return projectInfoList;
        }
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