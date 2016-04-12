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
    }
}