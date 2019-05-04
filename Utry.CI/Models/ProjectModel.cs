using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utry.Core.Domain;
using Utry.Framework.Mvc;
using System.Web.Mvc;

namespace Utry.CI.Models
{
    public class ProjectModel:BaseModel
    {
        public ProjectModel()
        {
            Project = new CIProject();
            PageList = new ProjectPageList();
            ProjectList = new List<CIProject>();
            User = new CIUser();
            CIReview = new CIReview();
            reviewlist = new List<CIReview>();
            Release = new CIRelease();
            ReleaseList = new List<CIRelease>();
            StatusSelectItem = new List<SelectListItem>();
        }

        public class ProjectPageList : BasePageableModel { };

        /// <summary>
        /// 项目列表集合
        /// </summary>
        public List<CIProject> ProjectList { get; set; }

        public ProjectPageList PageList { get; set; }

        public CIProject Project { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public CIUser User { get; set; }

        /// <summary>
        /// 评审
        /// </summary>
        public CIReview CIReview { get; set; }

        /// <summary>
        /// 评审列表集合
        /// </summary>
        public List<CIReview> reviewlist { get; set; }

        /// <summary>
        /// 搜索项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 搜索项目编号
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// 发布记录
        /// </summary>
        public CIRelease Release { get; set; }

        /// <summary>
        /// 发布记录列表
        /// </summary>
        public List<CIRelease> ReleaseList { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 发布状态
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 测试状态
        /// </summary>
        public string Teststatus { get; set; }

        /// <summary>
        /// 版本类型
        /// </summary>
        public string VersionType { get; set; }

        /// <summary>
        /// 状态下拉框数据集合
        /// </summary>
        public List<SelectListItem> StatusSelectItem { get; set; }

        /// <summary>
        /// 需求编号
        /// </summary>
        public string DemandCode { get; set; }

        public string code { get; set; }

    }
}