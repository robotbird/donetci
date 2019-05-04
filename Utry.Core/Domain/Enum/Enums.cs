using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utry.Core.Domain.Enum
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperateType
    {
        /// <summary>
        /// 添加
        /// </summary>
        Insert = 0,
        /// <summary>
        /// 更新
        /// </summary>
        Update = 1,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 2,
    }
    public enum NotifyType
    {
        Success,
        Error
    }
    /// <summary>
    /// 角色
    /// </summary>
    public enum Role 
    {
       管理员,
       开发组长,
       开发人员,
       版本发布人员,
       测试
    }
}
