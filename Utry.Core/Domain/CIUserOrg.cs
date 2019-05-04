using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utry.Core.Domain
{
    public class CIUserOrg
    {
        /// <summary>
        /// 小组名称
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// 小组编号
        /// </summary>
        public string OrgCode { get; set; }

        /// <summary>
        /// 父级编号
        /// </summary>
        public string ParentCode { get; set; }
    }
}
