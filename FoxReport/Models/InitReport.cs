﻿using FoxReport.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxReport.Models
{
    /// <summary>
    /// 初始加载时显示的各个Model列表
    /// </summary>
    public class InitReport
    {
        /// <summary>
        /// 周报名称
        /// </summary>
        public string ReportName = "";

        public List<SummaryTargetStrategy> SummaryTargetStrategyList = new List<SummaryTargetStrategy>();

        /// <summary>
        /// 二、项目概况
        /// </summary>
        public List<ProjectInfo> ProjectInfoList = new List<ProjectInfo>();
        
        /// <summary>
        /// 三、重点事务：产品事务
        /// </summary>
        public List<AffairProduct> AffairProductList = new List<AffairProduct>();

        /// <summary>
        /// 0,1,2分别保存SummaryTargetStrategyList，ProjectInfoList，AffairProductList
        /// </summary>
        public int[] totalCount = new int[3];
        /// <summary>
        /// 0,1,2分别保存SummaryTargetStrategyList，ProjectInfoList，AffairProductList
        /// </summary>
        public int[] totalPage = new int[3];

        /// <summary>
        /// 四、团队工作方式优化
        /// </summary>
        public  TeamworkInfo teamworkInfo = new TeamworkInfo();

        /// <summary>
        /// 五、需要的协助和支持
        /// </summary>
        public AssistInfo assistInfo = new AssistInfo();
        
    }
}