using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxReport.Helper
{
    public class WeekHelper
    {
        /// <summary>
        /// 指定日期是所属年的第几周
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static int GetWeekOfYear(DateTime day)
        {
            int week = 1;
            DateTime firstDay = new DateTime(day.Year, 1, 1);//指定年的第一天
            int dayOffset = day.DayOfYear - 1;
            int weekNum = (int)firstDay.DayOfWeek;// DayOfWeek 范围0-6
            week = (int)Math.Ceiling((dayOffset + weekNum) / 7.0);
            week = week == 0 ? 1 : week;//第一天1月1号是星期天，week==0
            return week;
        }

        /// <summary>
        /// 获取day所在周的开始，结束日期
        /// </summary>
        /// <param name="day"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void GetWeekStartEnd(DateTime day, out DateTime start, out DateTime end)
        {
            start = new DateTime();
            end = new DateTime();
            int week = GetWeekOfYear(day);
            int weekNum = (int)day.DayOfWeek;
            if (week == 1)
            {
                start = new DateTime(day.Year, 1, 1);
                if (start.DayOfWeek == DayOfWeek.Sunday)
                {
                    end = new DateTime(day.Year, 1, 8);
                }
                else
                {
                    end = start.AddDays(7 - (int)start.DayOfWeek);
                }
            }
            else
            {
                if (weekNum == 0)//day是星期天
                {
                    start = day.AddDays(-6);
                    end = day;
                }
                else
                {
                    start = day.AddDays(-(weekNum - 1));
                    end = day.AddDays(7 - weekNum);
                    if (end.Year > start.Year)//已经是下一年
                    {
                        end = new DateTime(day.Year, 12, 31);
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定年的最后一周的周数
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int GetLastWeekNum(int year)
        {
            DateTime lastDay = new DateTime(year, 12, 31);
            int lastWeek = GetWeekOfYear(lastDay);
            return lastWeek;
        }

        /// <summary>
        /// 根据yearWeek获取周的开始，结束日期
        /// </summary>
        /// <param name="yearWeek"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void GetWeekStartEnd(int yearWeek, out DateTime start, out DateTime end)
        {
            int year = yearWeek / 100;
            int week = yearWeek % 100;
            DateTime firstDay = new DateTime(year, 1, 1);
            if (week == 1)
            {
                GetWeekStartEnd(firstDay, out start, out end);
            }
            else
            {
                GetWeekStartEnd(firstDay.AddDays((week - 1) * 7), out start, out end);
            }
        }
    }
}