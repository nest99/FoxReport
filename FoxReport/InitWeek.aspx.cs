using FoxReport.Helper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FoxReport
{
    public partial class InitWeek : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonInitWeek_Click(object sender, EventArgs e)
        {
            InitYear();
        }

        protected void InitYear()
        {
            int year = 2016;
            int maxYear = 2030;
            for (; year <= maxYear; year++)
            {
                DateTime firstDay = new DateTime(year, 1, 1);
                int maxWeek = WeekHelper.GetLastWeekNum(year);
                int yearWeek;
                DateTime start, end;
                int week = 1;
                yearWeek = year * 100 + week;
                WeekHelper.GetWeekStartEnd(firstDay, out start, out end);
                InsertData(yearWeek, year, week, start, end);//插入第一周
                week++;
                int addDay = 7;
                if (firstDay.DayOfWeek == DayOfWeek.Sunday)//1月1号是星期天
                {
                    firstDay = firstDay.AddDays(8);//下一周星期一
                }
                else
                {
                    firstDay = firstDay.AddDays(addDay);
                }
                while (true)
                {
                    yearWeek = year * 100 + week;
                    WeekHelper.GetWeekStartEnd(firstDay, out start, out end);
                    InsertData(yearWeek, year, week, start, end);
                    week++;
                    firstDay = firstDay.AddDays(addDay);
                    if (firstDay.Year > year)
                    {
                        break;
                    }//如果是下一年，结束
                }
            }

            LabelMsg.Text = "生成成功";
        }

        protected void InsertData(int yearWeek, int year, int week, DateTime start, DateTime end)
        {
            string sql = "insert into WeekStartEndDay(YearWeek, YearNum, WeekNum, WeekStart, WeekEnd) " +
                " values(@YearWeek, @YearNum, @WeekNum, @WeekStart, @WeekEnd)";
            MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.Parameters.Add("@YearWeek", MySqlDbType.Int32).Value = yearWeek;
            cmd.Parameters.Add("@YearNum", MySqlDbType.Int32).Value = year;
            cmd.Parameters.Add("@WeekNum", MySqlDbType.Int32).Value = week;
            cmd.Parameters.Add("@WeekStart", MySqlDbType.DateTime).Value = start;
            cmd.Parameters.Add("@WeekEnd", MySqlDbType.DateTime).Value = end;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();                
            }
            catch (MySqlException e)
            {
                LabelMsg.Text += "插入数据库错误：" + e.Message;
            }
            finally
            {
                con.Close();
            }            
        }
    }
}
