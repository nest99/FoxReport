using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxReport.Models
{
    /// <summary>
    /// 一、概述：版本概况
    /// </summary>
    public class SummaryVersion : BaseInfo
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 核心需求
        /// </summary>
        public string Request { get; set; }
        /// <summary>
        /// 计划发布
        /// </summary>
        public string Publish { get; set; }
        /// <summary>
        /// 进展与风险
        /// </summary>
        public string Risk { get; set; }
    }
}