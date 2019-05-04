using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utry.Core.Domain;

namespace Utry.CI.Models
{
    public class UserOrgModel:BaseModel
    {
        public UserOrgModel()
        {
            UserOrgList = new List<CIUserOrg>();
        }

        public CIUserOrg UserOrg { get; set; }

        /// <summary>
        /// 小组集合
        /// </summary>
        public List<CIUserOrg> UserOrgList { get; set; }

        /// <summary>
        /// URlAction
        /// </summary>
        public string Action { get; set; }
    }
}