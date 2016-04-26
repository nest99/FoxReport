using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxReport.Models
{
    /// <summary>
    /// 每个用户的信息
    /// </summary>
    public class UserInfo
    {
        public string UserId
        {
            get;
            set;
        }
        public string UserName
        {
            get;
            set;
        }
        public int UserRole
        {
            get;
            set;
        }
    }
}