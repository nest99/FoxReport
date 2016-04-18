using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxReport.Models
{
    public class AffairProduct : BaseInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public string Classify
        {
            get;
            set;
        }
        /// <summary>
        /// 优先级
        /// </summary>
        public string Priority
        {
            get;
            set;
        }
        /// <summary>
        /// 负责人
        /// </summary>
        public string Tracker
        {
            get;
            set;
        }
        /// <summary>
        /// 工作计划
        /// </summary>
        public string Workplan
        {
            get;
            set;
        }
        /// <summary>
        /// 进展情况
        /// </summary>
        public string Progress
        {
            get;
            set;
        }     
        
    }
}