using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Utry.Core.Domain;
using Utry.CI.Models;
using Utry.Framework.Web;
using Utry.Core.Services;
using Utry.Framework.Utils;


namespace Utry.CI.Controllers
{
    public class ProjectController : BaseController
    {
        private ProjectService _projectservice = new ProjectService();
        private UserService _userService = new UserService();
        private ReviewService _reviewService = new ReviewService();
        private ReleaseService _releaseService = new ReleaseService();
        /// <summary>
        /// 项目列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ProjectList(ProjectModel model)
        {
            const int pageSize = 10;
            int count = 0;
            int pageIndex = PressRequest.GetInt("page", 1);

            var where = "";
            if (!string.IsNullOrEmpty(model.ProjectName) && Utils.IsSafeSqlString(model.ProjectName))
            {
                where += " and p.ProjectName like '%" + model.ProjectName + "%' ";
            }
            if (!string.IsNullOrEmpty(model.ProjectCode) && Utils.IsSafeSqlString(model.ProjectCode))
            {
                where += " and p.ProjectCode like '%" + model.ProjectCode + "%' ";
            }
            var list = _projectservice.GetProjectList(pageSize, pageIndex, out count, where);
            model.PageList.LoadPagedList(list);
            model.ProjectList = (List<CIProject>)list;
            return View(model);
        }

        [HttpGet]
        public ActionResult ProjectAdd(string id)
        {
            ProjectModel model = new ProjectModel();
            if (id != null)
            {
                model.Project = _projectservice.GetById(id);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProjectAdd(CIProject project)
        {
            ProjectModel model = new ProjectModel();
            model.Project = project;
            if (string.IsNullOrEmpty(project.ID))
            {
                project.ID = Guid.NewGuid().ToString();
                project.AddTime = DateTime.Now;
                project.UpdateTime = DateTime.Now;
                //model.Executive = PressRequest.GetFormString("hidUsername"); 暂时直接手填 不用人员树
                int num = _projectservice.InsertPro(project);
                if (num > 0)
                {
                    return Redirect(Url.Action("ProjectList", "Project"));
                }
                else
                {
                    ErrorNotification("添加失败");
                    return View(model);
                }
            }
            else
            {
               project.UpdateTime = DateTime.Now;
               int num =  _projectservice.UpdatePro(project);
               if (num > 0)
               {
                   return Redirect(Url.Action("ProjectList", "Project"));
               }
               else
               {
                   ErrorNotification("编辑失败");
                   return View(model);
               }
            }
            
           
        }

        public ActionResult DeleteProject(string id)
        {
            int result = _projectservice.DeletePro(id);
            if (result > 0)
            {
                SuccessNotification("删除成功");
                return Redirect(Url.Action("projectlist", "project"));
            }
            else
            {
                ErrorNotification("删除失败");
                return Redirect(Url.Action("projectlist", "project"));
            }
            
        }

        /// <summary>
        /// 验证项目编号的唯一性
        /// </summary>
        /// <param name="projectcode"></param>
        /// <returns></returns>
        public ActionResult IfExist(string projectcode)
        {
            var msg = _projectservice.IfExist(projectcode) ? "1" : "0";
            return Content(msg);
        }

        /// <summary>
        /// 项目详情 包括评审的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ProjectInfo(string id)
        {
            ProjectModel model = new ProjectModel();
            model.Project = _projectservice.GetById(id);
            model.ProjectList = _projectservice.GetProInfoList();
            model.code = id;
            //model.reviewlist = _reviewService.GetReviewListByProjectId(id);
            return View(model);
        }

        /// <summary>
        /// 项目详情tab页评审列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ProjectReviewInfo(string id, ProjectModel model)
        {
            //ProjectModel model = new ProjectModel();
            model.StatusSelectItem.Add(new SelectListItem { Text = "--选择状态--", Value = "", Selected = true });
            model.StatusSelectItem.Add(new SelectListItem { Text = "已评审", Value = "已评审" });
            model.StatusSelectItem.Add(new SelectListItem { Text = "未通过", Value = "未通过" });
            model.StatusSelectItem.Add(new SelectListItem { Text = "延期", Value = "延期" });
            model.StatusSelectItem.Add(new SelectListItem { Text = "不评审", Value = "不评审" });

            const int pageSize = 10;
            int count = 0;
            int pageIndex = PressRequest.GetInt("page", 1);

            var where = "";
            if (!string.IsNullOrEmpty(PressRequest.GetFormString("Status")))
            {
                where += " and r.Status ='" + PressRequest.GetFormString("Status") + "' ";
            }
            if (!string.IsNullOrEmpty(model.ProjectName) && Utils.IsSafeSqlString(model.ProjectName))
            {
                where += " and p.ProjectName like '%" + model.ProjectName + "%' ";
            }
            if (!string.IsNullOrEmpty(model.DemandCode) && Utils.IsSafeSqlString(model.DemandCode))
            {
                where += " and rp.DemandCode like '%" + model.DemandCode + "%' ";
            }
            if (!string.IsNullOrEmpty(model.BeginDate))
            {
                where += " and r.BeginDate >= '" + model.BeginDate + "' ";
            }
            if (!string.IsNullOrEmpty(model.EndDate))
            {
                where += " and r.BeginDate <= '" + model.EndDate + "' ";
            }

            var list = _reviewService.GetReviewListByProjectId(id,pageSize, pageIndex, out count, where);
            model.Project = _projectservice.GetById(id);
            model.ProjectList = _projectservice.GetProInfoList();
            model.PageList.LoadPagedList(list);
            model.reviewlist = (List<CIReview>)list;
            return View(model);
        }

        public ActionResult ProjectReleaseInfo(string id)
        {
            
            ProjectModel model = new ProjectModel();
            model.status = Request["Status"];
            model.VersionType = Request["VersionType"];
            model.Teststatus = Request["TestStatus"];
            const int pageSize = 10;
            int count = 0;
            int pageIndex = PressRequest.GetInt("page", 1);

            var where = "";
            //版本类型
            if (model.VersionType == "1")
            {
                where += " and Type ='正式版本'" ;
            }
            else if (model.VersionType == "0")
            {
                where += " and Type ='测试版本'";
            }
            //发布状态
            if (model.status == "1")
            {
                where += " and Status ='发布成功'";
            }
            else if (model.status == "0")
            {
                where += " and Status ='发布失败'";
            }
            //测试状态
            if (model.Teststatus == "1")
            {
                where += " and TestStatus ='测试通过'";
            }
            else if (model.Teststatus == "0")
            {
                where += " and TestStatus ='不通过'";
            }
            if (!string.IsNullOrEmpty(PressRequest.GetFormString("BeginDate")))
            {
                where += " and AddTime >= '" + PressRequest.GetFormString("BeginDate") + "' ";
            }
            if (!string.IsNullOrEmpty(PressRequest.GetFormString("EndDate")))
            {
                where += " and AddTime <= '" + PressRequest.GetFormString("EndDate") + "' ";
            }
            var list = _releaseService.GetReleaseList(id, pageSize, pageIndex, out count, where);
            model.Project = _projectservice.GetById(id);
            model.ProjectList = _projectservice.GetProInfoList();
            model.PageList.LoadPagedList(list);
            model.ReleaseList = (List<CIRelease>)list;
            model.BeginDate = PressRequest.GetFormString("BeginDate");
            model.EndDate = PressRequest.GetFormString("EndDate");
            return View(model);
        }

        public ActionResult ProjectVersionDownLoad(string id)
        {
            ProjectModel model = new ProjectModel();
            const int pageSize = 10;
            int count = 0;
            int pageIndex = PressRequest.GetInt("page", 1);

            var where = "";
            if (!string.IsNullOrEmpty(PressRequest.GetFormString("BeginDate")))
            {
                where += " and EndTime >= '" + PressRequest.GetFormString("BeginDate") + "' ";
            }
            if (!string.IsNullOrEmpty(PressRequest.GetFormString("EndDate")))
            {
                where += " and EndTime <= '" + PressRequest.GetFormString("EndDate") + "' ";
            }
            var list = _releaseService.GetReleaseListForDownLoad(id,pageSize, pageIndex, out count, where);
            model.Project = _projectservice.GetById(id);
            model.ProjectList = _projectservice.GetProInfoList();
            model.PageList.LoadPagedList(list);
            model.ReleaseList = (List<CIRelease>)list;
            model.BeginDate = PressRequest.GetFormString("BeginDate");
            model.EndDate = PressRequest.GetFormString("EndDate");
            return View(model);
        }

        #region  发布版本
        /// <summary>
        /// 添加测试版本发布记录
        /// </summary>
        /// <param name="ProjectCode">项目编号</param>
        /// <returns></returns>
        public ActionResult ReleaseAdd(string id)
        {
            ProjectModel model = new ProjectModel();
            model.Release.ID = Guid.NewGuid().ToString();
            model.Release.ProjectID = id;
            model.Release.Status = "发布中";
            model.Release.AddTime = DateTime.Now;
            model.Release.Operator = CurrentUser.FullName;
            model.Release.Type = "测试版本";

            //svn地址验证
            var project = _projectservice.GetById(id);
            var testSvnUri = project.ProjectSvnURL;//测试版本
            var releaseSvnUri = project.ProjectSvnURLRelease;//正式版本
            var svnService = new SvnService();
            if (!svnService.IsExistsUri(testSvnUri))
            {
                ErrorNotification("测试版本svn地址不正确");
            }
            else 
            {
                int num = _releaseService.InsertPro(model.Release);
                if (num > 0)
                {
                    SuccessNotification("项目已成功进入待发布状态");
                    return Redirect(Url.Action("projectreleaseinfo", "project", new { id = id }));
                }
                else
                {
                    ErrorNotification("项目进入待发布状态失败");
                    return Redirect(Url.Action("projectreleaseinfo", "project", new { id = id }));
                }
            }
            return Redirect(Url.Action("projectreleaseinfo", "project", new { id = id }));
        }

        /// <summary>
        /// 添加正式版本发布记录
        /// </summary>
        /// <param name="ProjectCode">项目编号</param>
        /// <returns></returns>
        public ActionResult ReleaseFormalAdd(string id)
        {
            ProjectModel model = new ProjectModel();
            model.Release.ID = Guid.NewGuid().ToString();
            model.Release.ProjectID = id;
            model.Release.Status = "发布中";
            model.Release.AddTime = DateTime.Now;
            model.Release.Operator = CurrentUser.FullName;
            model.Release.Type = "正式版本";

            //svn地址验证
            var project = _projectservice.GetById(id);
            var testSvnUri = project.ProjectSvnURL;//测试版本
            var releaseSvnUri = project.ProjectSvnURLRelease;//正式版本
            var svnService = new SvnService();
            if (!svnService.IsExistsUri(releaseSvnUri))
            {
                ErrorNotification("正式版本svn地址不正确");
            }
            else
            {
                int num = _releaseService.InsertPro(model.Release);
                if (num > 0)
                {
                    SuccessNotification("项目已成功进入待发布状态");
                    return Redirect(Url.Action("projectreleaseinfo", "project", new { id = id }));
                }
                else
                {
                    ErrorNotification("项目进入待发布状态失败");
                    return Redirect(Url.Action("projectreleaseinfo", "project", new { id = id }));
                }
            }
            return Redirect(Url.Action("projectreleaseinfo", "project", new { id = id }));
        }
        #endregion

        /// <summary>
        /// 查看日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShowLog(string id)
        {
            ProjectModel model = new ProjectModel();
            model.Release = _releaseService.GetLogByID(id);
            return View(model);
        }

        /// <summary>
        /// 查看备注
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShowRemark(string id)
        {
            ProjectModel model = new ProjectModel();
            model.Release = _releaseService.GetLogByID(id);
            return View(model);
        }

        /// <summary>
        /// 设置测试状态
        /// </summary>
        /// <returns></returns>
        public ActionResult SetTestStatus(string proid,string id,int ifpass)
        {
            ProjectModel model = new ProjectModel();
            model.Release.ID = id;
            if (ifpass == 1)
            {
                model.Release.TestStatus = "测试通过";
            }
            else
            {
                model.Release.TestStatus = "不通过";
            }
            int num = _releaseService.SetTestStatus(model.Release);
            if (num > 0)
            {
                SuccessNotification("设置成功");
                return Redirect(Url.Action("projectreleaseinfo", "project", new { id = proid }));
            }
            else
            {
                ErrorNotification("设置失败");
                return Redirect(Url.Action("projectreleaseinfo", "project", new { id = proid }));
            }

        }
        /// <summary>
        /// 异步获取发布状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetSyncStatus(string id)
        {
            var release = _releaseService.GetById(id);
            return Json(release, JsonRequestBehavior.AllowGet);
        }

    }
}
