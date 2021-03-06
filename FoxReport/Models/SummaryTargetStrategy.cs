﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxReport.Models
{
    /// <summary>
    /// 一、概述：目标与策略
    /// </summary>   
    public class SummaryTargetStrategy : BaseInfo
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
        /// 现状
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 目标
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// 策略与措施
        /// </summary>
        public string Strategy { get; set; }
    }
}