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
            
            MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
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
                    t.UserId = int.Parse(reader["UserId"].ToString());
                    t.Week = int.Parse(reader["Week"].ToString());
                    t.IsForeign = int.Parse(reader["IsForeign"].ToString());
                    initShow.SummaryTargetStrategyList.Add(t);
                }
                reader.Close();

                reader = MySqlHelper.ExecuteReader(con, "select * from Detail_BizTarget " + condition);
                while (reader.Read())
                {
                    DetailBizTarget b = new DetailBizTarget();
                    b.Id = int.Parse(reader["Id"].ToString());
                    b.KeyWork = reader["KeyWork"].ToString();
                    b.ProblemAnalyze = reader["ProblemAnalyze"].ToString();
                    b.ProjectName = reader["ProjectName"].ToString();
                    b.RecentTarget = reader["RecentTarget"].ToString();
                    b.UserId = int.Parse(reader["UserId"].ToString());
                    b.Week = int.Parse(reader["Week"].ToString());
                    b.IsForeign = int.Parse(reader["IsForeign"].ToString());
                    initShow.DetailBizTargetList.Add(b);
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

            MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
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
            MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
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
                    t.UserId = int.Parse(reader["UserId"].ToString());
                    t.Week = int.Parse(reader["Week"].ToString());
                    t.IsForeign = int.Parse(reader["IsForeign"].ToString());
                    targetList.Add(t);
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

            return targetList;
        }

        public static List<SummaryVersion> GetSummaryVersion(int userId, int week, int isForeign)
        {
            List<SummaryVersion> versionList = new List<SummaryVersion>();
            MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
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
                    v.UserId = int.Parse(reader["UserId"].ToString());
                    v.Week = int.Parse(reader["Week"].ToString());
                    v.IsForeign = int.Parse(reader["IsForeign"].ToString());
                    versionList.Add(v);
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

            return versionList;
        }

        public static List<SummaryFeedback> GetSummaryFeedback(int userId, int week, int isForeign)
        {
            List<SummaryFeedback> feedbackList = new List<SummaryFeedback>();
            MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
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
                    f.UserId = int.Parse(reader["UserId"].ToString());
                    f.Week = int.Parse(reader["Week"].ToString());
                    f.IsForeign = int.Parse(reader["IsForeign"].ToString());
                    feedbackList.Add(f);
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

            return feedbackList;
        }
    }
}