using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Utry.Core.Domain;


namespace Utry.CI.Models
{
    public class CheckItemModel:BaseModel
    {
        public CheckItemModel() 
        {
            CheckItem = new CICheckItem();
            plan = new CIVersionPlan();
            PlanSelectItem = new List<SelectListItem>();
        }
        /// <summary>
        /// 需求
        /// </summary>
        public CICheckItem CheckItem { get; set; }
        /// <summary>
        /// 版本发布计划
        /// </summary>
        public CIVersionPlan plan { get; set; }
        /// <summary>
        /// selectitem
        /// </summary>
        public List<SelectListItem> PlanSelectItem { get; set; }
    }
}