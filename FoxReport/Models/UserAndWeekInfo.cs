using FoxReport.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxReport.Models
{
    /// <summary>
    /// 查询Div显示的用户列表和周信息
    /// </summary>
    public class UserAndWeekInfo
    {
        /// <summary>
        /// 所有用户列表
        /// </summary>
        public List<UserInfo> UserInfoList = CacheFoxData.UserList;

        private List<WeekInfo> weekInfoList = null;
        /// <summary>
        /// 当前日期前约20周,后2周的列表
        /// </summary>
        public List<WeekInfo> WeekInfoList
        {
            get
            {
                if (weekInfoList == null)
                {
                    int start, end;
                    int currentWeek = WeekHelper.GetWeekOfYear(DateTime.Now);
                    int currentYear = DateTime.Now.Year;
                    if (currentWeek <= 20)
                    {
                        //所有年都按一年52周算
                        start = (currentYear - 1) * 100 + (52 - currentWeek);
                        end = currentYear * 100 + currentWeek + 2;//后2周

                    }
                    else
                    {
                        start = currentYear * 100 + (currentWeek - 20);
                        if (currentWeek > 50)
                        {
                            end = (currentYear + 1) * 100 + 2;//下一年的前2周
                        }
                        else
                        {
                            end = currentYear * 100 + (currentWeek + 2);
                        }
                    }
                    var list = from w in CacheFoxData.WeekList
                               where w.YearWeek >= start && w.YearWeek <= end
                               select w;
                    weekInfoList = list.ToList<WeekInfo>();
                }
                return weekInfoList;
            }
        }
    }
}