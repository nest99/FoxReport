using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxReport.Models
{
    public class SummaryFeedback : BaseInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public string Seq { get; set; }
        /// <summary>
        /// 平台
        /// </summary>
        public string Platform
        {
            get;
            set;
        }
        /// <summary>
        /// 问题概要
        /// </summary>
        public string Issue
        {
            get;
            set;
        }
        /// <summary>
        /// 跟进人
        /// </summary>
        public string Tracker
        {
            get;
            set;
        }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 跟进情况
        /// </summary>
        public string TrackInfo { get; set; }
    }
}