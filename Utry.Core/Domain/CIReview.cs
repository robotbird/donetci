using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utry.Core.Domain
{
    public class CIReview
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// 评审目的
        /// </summary>
        public string Purpose { get; set; }

        /// <summary>
        /// 评审方式
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 评审资料
        /// </summary>
        public string Attachment { get; set; }

        /// <summary>
        /// 评审结论
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 评审规模
        /// </summary>
        public string Scale { get; set; }

        /// <summary>
        /// 是否复审
        /// </summary>
        public string IfReview { get; set; }

        /// <summary>
        /// 评审会议准备工作量
        /// </summary>
        public string PrepareTime { get; set; }

        /// <summary>
        /// 评审人员
        /// </summary>
        public string Member { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }


        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 需求编号
        /// </summary>
        public string DemandCode { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public string ProjectID { get; set; }

        /// <summary>
        /// 评审问题描述
        /// </summary>
        public string Description { get; set; }
    }
}
