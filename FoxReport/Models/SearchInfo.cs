using FoxReport.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxReport.Models
{
    /// <summary>
    /// 查询条件类
    /// </summary>
    public class SearchInfo
    {
        /// <summary>
        /// 从每个表的默认字段模糊查询的值。如：项目名称
        /// </summary>
        public string FieldValue
        {
            get;
            set;
        }
        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <returns></returns>
        public string GetSearchCondition()
        { 
            string condition = " IsForeign=" + IsForeign.ToString();

            if (UserId.Trim() != "")
            {
                condition += " AND UserId='" + UserId + "' ";
            }

            if (IsOneWeek)
            {
                condition += " AND Week=" + Week.ToString();
            }
            else
            {
                if (StartDate.Trim() != "")
                {
                    DateTime start;
                    if (DateTime.TryParse(EndDate, out start))
                    {
                        condition += " AND Week >= " + WeekHelper.GetWeekOfYear(start).ToString();
                    }
                }
                if (EndDate.Trim() != "")
                {
                    DateTime end;
                    if (DateTime.TryParse(EndDate, out end))
                    {
                        condition += " AND Week <= " + WeekHelper.GetWeekOfYear(end).ToString();
                    }
                }
            }

            return condition;
        }
        /// <summary>
        /// 用户Id, 负责人
        /// </summary>
        public string UserId
        {
            get;
            set;
        }
        private bool isOneWeek = true;
        /// <summary>
        /// 是按周查询还是按时间段查询。true按指定Week，false按时间段
        /// </summary>
        public bool IsOneWeek
        {
            get
            {
                return isOneWeek;
            }
            set
            {
                isOneWeek = value;
            }
        }
        private int week = 0;
        /// <summary>
        /// 年周，如210616
        /// </summary>
        public int Week
        {
            get
            {
                return week;
            }
            set
            {
                IsOneWeek = true;
                week = value;
            }
        }
        private string startDate = "";
        private string endDate = "";
        /// <summary>
        /// 查询的开始时间
        /// </summary>
        public string StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                IsOneWeek = false;
                startDate = value;
            }
        }
        /// <summary>
        /// 查询的结束时间
        /// </summary>
        public string EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                IsOneWeek = false;
                endDate = value;
            }
        }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex
        {
            get;
            set;
        }
        private int pageSize = 10;
        /// <summary>
        /// 每页多少条记录
        /// </summary>
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                if (value < 1)
                {
                    pageSize = 10;
                }
                else
                {
                    pageSize = value;
                }
            }
        }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount
        {
            get;
            set;
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage
        {
            get
            {
                if(PageSize < 1)
                {
                    PageSize = 10;
                }
                return TotalCount % PageSize == 0 ? TotalCount / PageSize : TotalCount / PageSize + 1;
            }
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