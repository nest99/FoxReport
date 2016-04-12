using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxReport.Models
{
    /// <summary>
    /// 初始加载时显示的各个Model列表
    /// </summary>
    public class InitShow
    {
        public List<SummaryTargetStrategy> SummaryTargetStrategyList = new List<SummaryTargetStrategy>();
        public List<DetailBizTarget> DetailBizTargetList = new List<DetailBizTarget>();
    }
}