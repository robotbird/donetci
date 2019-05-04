using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utry.Framework;
using Utry.Framework.Web;
using Utry.Framework.Configuration;
using Utry.Framework.Utils;
using Utry.Core.Domain;
using Utry.Core.Domain.Enum;
using Utry.Core.Services;
using Utry.CI.Models;

namespace Utry.CI.Controllers
{
    public class PlanController : BaseController
    {
        private VersionPlanService _planService = new VersionPlanService();
        #region 版本计划
        /// <summary>
        /// 计划列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List(PlanListModel model)
        {
            const int pageSize = 10;
            int count = 0;
            int pageIndex = PressRequest.GetInt("page", 1);

            var where = "";
            if (!string.IsNullOrEmpty(model.PlanCode)) 
            {
                where += " and PlanCode like '%"+model.PlanCode+"%' ";
            }

            var list = _planService.GetPlanPageList(pageSize, pageIndex, out count, where);
            model.PageList.LoadPagedList(list);
            model.ItemList = (List<CIVersionPlan>)list;
            return View(model);
        }
        /// <summary>
        /// 计划编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(string Id)
        {
            var model = new PlanModel();
            if (Id != null) 
            {
                model.plan = _planService.GetPlan(Id);
            }
            return View(model);
        }
        /// <summary>
        /// 保存计划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(CIVersionPlan plan)
        {
            var model = new PlanModel();
            model.plan = plan;
            bool success = true;

            #region 格式验证
            if (string.IsNullOrEmpty(plan.BeginTime) || string.IsNullOrEmpty(plan.EndTime)) 
            {
                ErrorNotification("请选择时间范围");
                return View(model);
            }
            if(string.IsNullOrEmpty(plan.OpenDate))
            {
                ErrorNotification("请选择开放时间");
                return View(model);
            }
            if(string.IsNullOrEmpty(plan.Note))
            {
                ErrorNotification("请填写计划说明");
                return View(model);
            }
            #endregion

            //新增计划
            if (string.IsNullOrEmpty(plan.ID))
            {
                plan.AddTime = DateTime.Now;
                plan.UpdateTime = DateTime.Now;
                plan.UserName = CurrentUser.UserName;
                var begintime = Convert.ToDateTime(plan.BeginTime);
                var endtime = Convert.ToDateTime(plan.EndTime);
                plan.PlanCode = "CCMS" + begintime.ToString("yyyyMMdd").Substring(2, 6) + "_" + endtime.ToString("yyyyMMdd").Substring(2, 6);
                int num = 0;
                if (success)
                {
                    plan.ID = Guid.NewGuid().ToString();
                    num = _planService.InsertVersionPlan(plan);
                }
                if (num > 0)
                {
                    SuccessNotification("添加成功");
                }
                else
                {
                    ErrorNotification("添加失败");
                    return View(model);
                }
            }
            else//修改计划
            {
                var oldmodel = _planService.GetPlan(plan.ID); ;
                plan.AddTime = oldmodel.AddTime;
                plan.UpdateTime = DateTime.Now;
                plan.UserName = oldmodel.UserName;
                int num = 0;
                if (success)
                {
                    num = _planService.UpdateVersionPlan(plan);
                }
                if (num > 0)
                {
                    SuccessNotification("修改成功");
                }
                else
                {
                    ErrorNotification("修改失败");
                    return View(model);
                }
            }
            if (success)
            {
                return Redirect(Url.Action("list", "plan"));
            }
            else
            {
                return View(model);
            }
        }

        /// <summary>
        /// 删除计划
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(string Id) 
        {
            var num = 0;
            num = _planService.DeleteVersionPlan(Id) ;
            if (num > 0)
            {
                SuccessNotification("删除成功");
                return Redirect(Url.Action("list", "plan"));
            }
            else
            {
                ErrorNotification("删除失败");
                return Redirect(Url.Action("list", "plan"));
            }
        }
        #endregion

    }
}
