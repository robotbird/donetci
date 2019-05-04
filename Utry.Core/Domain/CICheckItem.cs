using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utry.Core.Domain
{
    public class CICheckItem
    {
        /// <summary>
        /// 主键guid
        /// </summary>		
        public string ID { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>		
        public string VersionCode { get; set; }
        /// <summary>
        /// 需求编号
        /// </summary>		
        public string DemandCode { get; set; }
        /// <summary>
        /// 代码清单
        /// </summary>		
        public string CodeList { get; set; }
        /// <summary>
        /// 附件
        /// </summary>		
        public string Attachment { get; set; }
        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; }
        /// <summary>
        /// 开发人员
        /// </summary>		
        public string Developer { get; set; }
        /// <summary>
        /// 测试人员
        /// </summary>		
        public string Tester { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>		
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>		
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>		
        public string Status { get; set; }
        /// <summary>
        /// 验证方法描述
        /// </summary>		
        public string ValidateNote { get; set; }
        /// <summary>
        /// 部署方法注意事项
        /// </summary>		
        public string DeployNote { get; set; }
        /// <summary>
        /// 从vss获取此item代码次数
        /// </summary>
        public int GetVssCnt { get; set; }
        /// <summary>
        /// 提交物类型
        /// </summary>
        public string ItemType { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 需求或者bug编号对应的版本计划id
        /// </summary>
        public string PlanId { get; set; }

        #region 计划表字段
        /// <summary>
        /// 计划编号
        /// </summary>
        public string PlanCode { get; set; }
        #endregion

        #region utmp字段
        /// <summary>
        /// 需求状态
        /// </summary>
        public string demandstate { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string versionnum { get; set; }
        #endregion

    }
}
