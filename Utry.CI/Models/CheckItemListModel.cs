using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utry.Core.Domain;
using Utry.Framework.Mvc;

namespace Utry.CI.Models
{
    public class CheckItemListModel:BaseModel
    {
        public CheckItemListModel() 
        {
            PageList = new CheckItemPageList();
            PlanList = new List<CIVersionPlan>();
            ItemList = new List<CICheckItem>();
            PlanSelectItem = new List<SelectListItem>();
        }
        public class CheckItemPageList : BasePageableModel { };

        /// <summary>
        /// list
        /// </summary>
        public List<CICheckItem> ItemList { get; set; }

        public CheckItemPageList PageList { get; set; }
        /// <summary>
        /// 版本状态
        /// </summary>
        public string VersionState { get; set; }
        /// <summary>
        /// 测试状态
        /// </summary>
        public string TestState { get; set; }
        /// <summary>
        /// 代码是否重复状态
        /// </summary>
        public string CodeState { get; set; }
        /// <summary>
        /// 是否新增代码
        /// </summary>
        public string IsNewCode { get; set; }
        /// <summary>
        /// 需求(bug)编号
        /// </summary>
        public string DemandCode { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string datefrom { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string dateto { get; set; }
        /// <summary>
        /// 计划编号
        /// </summary>
        public string PlanId { get; set; }

        /// <summary>
        /// 下拉框选中状态
        /// </summary>
        /// <param name="val">model value</param>
        /// <param name="value">option value</param>
        /// <returns></returns>
        public string OptionSelected(object val,object value) 
        {
            if (val == null) 
            {
                return "";
            }

            if (val.ToString() == value.ToString())
            {
                return "selected=\"selected\"";
            }
            else 
            {
                return "";
            }
        }

        /// <summary>
        /// selectitem
        /// </summary>
        public List<SelectListItem> PlanSelectItem { get; set; }
        /// <summary>
        /// 版本计划
        /// </summary>
        public List<CIVersionPlan> PlanList { get; set; }
        /// <summary>
        /// 根据计划编号 获取计划开放时间 判断是否能编辑
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsCanEdit(CICheckItem item) 
        {
             //如果当期时间在此记录的计划时间范围之类才允许修改或者删除
            var list = from plan in this.PlanList where plan.ID == item.PlanId select plan;
            if (list != null&&list.Count()>0) 
            {
                var p = list.First();
                return (DateTime.Now > Convert.ToDateTime(p.BeginTime) && DateTime.Now < Convert.ToDateTime(p.EndTime)); 
            }
            return false;
        }
    }
}