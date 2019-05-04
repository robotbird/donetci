using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utry.Core.Domain;
using Utry.Framework.Mvc;

namespace Utry.CI.Models
{
    public class PlanListModel : BaseModel
    {
        public PlanListModel() 
        {
            PageList = new PlanPageList();
            ItemList = new List<CIVersionPlan>();
        }
        public class PlanPageList : BasePageableModel { };

        public List<CIVersionPlan> ItemList { get; set; }

        public PlanPageList PageList { get; set; }
        /// <summary>
        /// 计划编号
        /// </summary>
        public string PlanCode { get; set; }
    }
}