using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxReport.Models
{
    /// <summary>
    /// 二、项目概况
    /// </summary>
    public class ProjectInfo : BaseInfo
    {
        /// <summary>
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
        /// 工作进展
        /// </summary>
        public string Progress
        {
            get;
            set;
        }
        /// <summary>
        /// 目标与重点
        /// </summary>
        public string Target
        {
            get;
            set;
        }
        /// <summary>
        /// 团队工作方式优化
        /// </summary>
        public string Teamwork
        {
            get;
            set;
        }
        /// <summary>
        /// 版本详情
        /// </summary>
        public string VersionDetail
        {
            get;
            set;
        }
        /// <summary>
        /// 版本质量分析
        /// </summary>
        public string VersionQuality
        {
            get;
            set;
        }       
        
    }
}