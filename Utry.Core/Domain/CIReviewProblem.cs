using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utry.Core.Domain
{
    public class CIReviewProblem
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 关联评审主键
        /// </summary>
        public string ReviewID { get; set; }

        /// <summary>
        /// 需求编号
        /// </summary>
        public string DemandCode { get; set; }

        /// <summary>
        /// 问题描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 问题提出人
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// 最后期限
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// 解决人员
        /// </summary>
        public string Solver { get; set; }

        /// <summary>
        /// 是否解决
        /// </summary>
        public string IfSolve { get; set; }

        /// <summary>
        /// 开发工时
        /// </summary>
        public string DevelopTime { get; set; }

        /// <summary>
        /// 设计工时
        /// </summary>
        public string DesignTime { get; set; }

        /// <summary>
        /// 测试工时
        /// </summary>
        public string TestTime { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }


        #region utmp字段
        /// <summary>
        /// utmp状态
        /// </summary>
        public string demandstate { get; set; }
        #endregion
    }
}
