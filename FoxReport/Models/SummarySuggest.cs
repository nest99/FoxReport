using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxReport.Models
{
    public class SummarySuggest : BaseInfo
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
        /// 建议内容
        /// </summary>
        public string SuggestContent
        {
            get;
            set;
        }
        /// <summary>
        /// 反馈用户数
        /// </summary>
        public string UserCount
        {
            get;
            set;
        }
        /// <summary>
        /// 影响用户体验描述
        /// </summary>
        public string Issue
        {
            get;
            set;
        }
        
        /// <summary>
        /// 跟进情况
        /// </summary>
        public string TrackInfo { get; set; }
    }
}