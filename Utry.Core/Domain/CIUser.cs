using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utry.Core.Domain
{
    public class CIUser
    {

        /// <summary>
        /// 用户名
        /// </summary>		
        public string UserName { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>		
        public string Email { get; set; }
        /// <summary>
        /// utmp用户名
        /// </summary>		
        public string UTMPName { get; set; }
        /// <summary>
        /// 手机
        /// </summary>		
        public string Mobile { get; set; }
        /// <summary>
        /// 角色
        /// </summary>		
        public string Role { get; set; }
        /// <summary>
        /// 密码
        /// </summary>		
        public string PassWord { get; set; }
        /// <summary>
        /// 状态
        /// </summary>		
        public string Status { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegTime { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 小组编号
        /// </summary>
        public string OrgCode { get; set; }

        /// <summary>
        /// 小组信息
        /// </summary>
        public string OrgName { get; set; }
    }
}
