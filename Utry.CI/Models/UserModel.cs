using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utry.Core.Domain;

namespace Utry.CI.Models
{
    public class UserModel
    {
        /// <summary>
        /// user对象
        /// </summary>
        public CIUser User { get; set; }

        public List<CIUser> Users { get; set; }

        public List<CIUserOrg> UserOrg { get; set; }

        /// <summary>
        /// 执行的Action 是新增还是编辑
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 小组列表项
        /// </summary>
        public List<SelectListItem> UserSelect { get; set; }

        /// <summary>
        /// 角色列表项
        /// </summary>
        public List<SelectListItem> UserRole { get; set; }
    }
}