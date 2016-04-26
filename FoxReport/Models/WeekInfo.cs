using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxReport.Models
{
    /// <summary>
    /// 每个周的信息
    /// </summary>
    public class WeekInfo
    {
        public int YearWeek
        {
            get;
            set;
        }
        public int Year
        {
            get;
            set;
        }
        public int Week
        {
            get;
            set;
        }
        public DateTime WeekStart
        {
            get;
            set;
        }
        public DateTime WeekEnd
        {
            get;
            set;
        }
    }
}