using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxReport.Models
{
    public class ReportData
    {
        /// <summary>
        /// 周报名称
        /// </summary>
        public string ReportName = "";

        public List<SummaryTargetStrategy> SummaryTargetStrategyList = new List<SummaryTargetStrategy>();
        public List<SummaryVersion> SummaryVersionList = new List<SummaryVersion>();
        public List<SummaryFeedback> SummaryFeedbackList = new List<SummaryFeedback>();
        public List<SummarySuggest> SummarySuggestList = new List<SummarySuggest>();

        /// <summary>
        /// 二、项目概况
        /// </summary>
        public List<ProjectInfo> ProjectInfoList = new List<ProjectInfo>();

        /// <summary>
        /// 三、重点事务：产品事务
        /// </summary>
        public List<AffairProduct> AffairProductList = new List<AffairProduct>();

        /// <summary>
        /// 四、团队工作方式优化
        /// </summary>
        public List<TeamworkInfo> TeamworkInfoList = new List<TeamworkInfo>();

        /// <summary>
        /// 五、需要的协助和支持
        /// </summary>
        public List<AssistInfo> AssistInfoList = new List<AssistInfo>();
    }
}