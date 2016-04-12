using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxReport.Models
{
    /// <summary>
    /// 业务目标
    /// </summary>
    public class DetailBizTarget : BaseInfo
    {/// <summary>
        /// 项目Id
        /// </summary>
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get;
            set;
        }
        /// <summary>
        /// 问题分析
        /// </summary>
        public string ProblemAnalyze
        {
            get;
            set;
        }
        /// <summary>
        /// 近期目标
        /// </summary>
        public string RecentTarget
        {
            get;
            set;
        }
        /// <summary>
        /// 重点工作
        /// </summary>
        public string KeyWork
        {
            get;
            set;
        }
        
    }
}