using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Utry.Core.Domain;
using Utry.Framework.Mvc;
using System.Web.Mvc;

namespace Utry.CI.Models
{
    public class ReviewModel
    {
        public ReviewModel()
        {
            ReviewList = new List<CIReview>();
            Review = new CIReview();
            PageList = new ReviewPageList();
            StatusSelectItem = new List<SelectListItem>();
            MethodSelectItem = new List<SelectListItem>();
            User = new CIUser();
            UserList = new List<CIUser>();
            IfReviewSelectItem = new List<SelectListItem>();

            ReviewPro = new CIReviewProblem();
            DemandList = new List<CIDemand>();
            IfSolveSelectItem = new List<SelectListItem>();
            DemandSelectItem = new List<SelectListItem>();
            Demand = new CIDemand();
            DemandList = new List<CIDemand>();
        }

        public class ReviewPageList : BasePageableModel { };

        /// <summary>
        /// 评审数据集合
        /// </summary>
        public List<CIReview> ReviewList { get; set; }

        /// <summary>
        /// 项目数据集合
        /// </summary>
        public List<CIProject> ProjectList { get; set; }

        /// <summary>
        /// 状态下拉框数据集合
        /// </summary>
        public List<SelectListItem> StatusSelectItem { get; set; }

        /// <summary>
        /// 项目下拉框数据集合
        /// </summary>
        public List<SelectListItem> ProjectSelectItem { get; set; }

        /// <summary>
        /// 评审方式下拉框集合
        /// </summary>
        public List<SelectListItem> MethodSelectItem { get; set; }

        public List<SelectListItem> IfReviewSelectItem { get; set; }

        public ReviewPageList PageList { get; set; }

        public CIReview Review { get; set; }

        public CIUser User { get; set; }

        /// <summary>
        /// 用户集合
        /// </summary>
        public List<CIUser> UserList { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 是否是添加还是编辑操作
        /// </summary>
        public string IfAdd { get; set; }

        /// <summary>
        /// 判断是否是项目详情页面调用
        /// </summary>
        public string IfPro { get; set; }

        /// <summary>
        /// 项目主键ID
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// 评审方式
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 是否复审
        /// </summary>
        public string IfReview { get; set; }

        public string Action { get; set; }

        #region "搜索"

        /// <summary>
        /// 评审状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 需求编号
        /// </summary>
        public string DemandCode { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndDate { get; set; }

        #endregion

        
  

        /// <summary>
        /// 评审问题
        /// </summary>
        public CIReviewProblem ReviewPro { get; set; }

        /// <summary>
        /// 需求列表下拉框
        /// </summary>
        public List<SelectListItem> DemandSelectItem { get; set; }

        /// <summary>
        /// 是否解决下拉框
        /// </summary>
        public List<SelectListItem> IfSolveSelectItem { get; set; }

        public List<CIReviewProblem> ReviewProList { get; set; }

        /// <summary>
        /// 是否解决
        /// </summary>
        public string IfSolve { get; set; }

        /// <summary>
        /// 评审表主键
        /// </summary>
        public string ReviewID { get; set; }

        public CIDemand Demand { get; set; }

        public List<CIDemand> DemandList { get; set; }
    }
}