using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxReport.Models
{
    public class BaseInfo
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId
        {
            get;
            set;
        }
        /// <summary>
        /// 年周，如210616
        /// </summary>
        public int Week
        {
            get;
            set;
        }
        /// <summary>
        /// 国内=0， 国外=1
        /// </summary>
        public int IsForeign
        {
            get;
            set;
        }
        /// <summary>
        /// 序号
        /// </summary>
        public int OrderNum
        {
            get;
            set;
        }
    }
}