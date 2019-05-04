using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utry.Core.Domain
{
   public class CIVersionPlan
    {
       /// <summary>
       /// 主键
       /// </summary>
       public string ID { get; set; }
       /// <summary>
       /// 开始时间
       /// </summary>		
       public string BeginTime { get; set; }
       /// <summary>
       /// 结束时间
       /// </summary>		
       public string EndTime { get; set; }
       /// <summary>
       /// 计划编号
       /// </summary>		
       public string PlanCode { get; set; }
       /// <summary>
       /// 状态
       /// </summary>		
       public string Status { get; set; }
       /// <summary>
       /// 说明
       /// </summary>		
       public string Note { get; set; }
       /// <summary>
       /// 添加时间
       /// </summary>		
       public DateTime AddTime { get; set; }
       /// <summary>
       /// 更新时间
       /// </summary>		
       public DateTime UpdateTime { get; set; }
       /// <summary>
       /// 用户名
       /// </summary>		
       public string UserName { get; set; }
       /// <summary>
       /// 开放日期
       /// </summary>
       public string OpenDate { get; set; }
    }
}
