using FoxReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxReport.Helper
{
    /// <summary>
    /// 缓存数据库数据信息
    /// </summary>
    public class CacheFoxData
    {
        private static List<UserInfo> userList = SqlDbHelper.GetUsersInfo();
        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        public static List<UserInfo> UserList
        {
            get
            {
                return userList;
            }
        }
        /// <summary>
        /// 重新从数据库获取数据
        /// </summary>
        public static void RefreshUserInfo()
        {
            userList = SqlDbHelper.GetUsersInfo();
        }
        private static List<WeekInfo> weekList = SqlDbHelper.GetWeekInfo();
        /// <summary>
        /// 获取所有周信息
        /// </summary>
        public static List<WeekInfo> WeekList
        {
            get
            {
                return weekList;
            }
        }
        /// <summary>
        /// 重新从数据库获取数据
        /// </summary>
        public static void RefreshWeekInfo()
        {
            weekList = SqlDbHelper.GetWeekInfo();
        }
    }
}