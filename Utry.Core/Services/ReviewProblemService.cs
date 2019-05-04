using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Utry.Core.Repositories.Repository;
using Utry.Core.Repositories.IRepository;
using Utry.Core.Domain;
using Utry.Framework.Web;
using Utry.Framework.Configuration;
using Utry.Framework.Utils;
using Utry.Framework.Mvc;

namespace Utry.Core.Services
{
    public class ReviewProblemService
    {
        private IReviewProblemRepository _reviewproblemRepositpry;

        #region "构造函数"

        public ReviewProblemService()
            : this(new ReviewProblemRepository())
        { 
        }

        public ReviewProblemService(IReviewProblemRepository reviewproblemRepository)
        {
            this._reviewproblemRepositpry = reviewproblemRepository;
        }
        #endregion

        /// <summary>
        /// 添加评审
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public int InsertPro(CIReviewProblem reviewpro)
        {
            return _reviewproblemRepositpry.Insert(reviewpro);
        }

        /// <summary>
        /// 编辑需求
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public int UpdatePro(CIReviewProblem reviewpro)
        {
            return _reviewproblemRepositpry.Update(reviewpro);
        }

        /// <summary>
        /// 删除需求
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public int DeletePro(string id)
        {
            var project = _reviewproblemRepositpry.GetById(id);
            if (project == null)
            {
                return 0;
            }
            int num = _reviewproblemRepositpry.Delete(project);
            return num;
        }

        /// <summary>
        /// 根据ID获取需求信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CIReviewProblem GetById(string id)
        {
            return _reviewproblemRepositpry.GetById(id);
        }

        /// <summary>
        /// 获取所有的需求问题
        /// </summary>
        /// <returns></returns>
        public List<CIReviewProblem> GetAll(string id)
        {
            return _reviewproblemRepositpry.GetReviewProByReviewID(id);
        }


        /// <summary>
        /// 根据需求编号获取问题
        /// </summary>
        /// <returns></returns>
        public List<CIReviewProblem> GetReproByDemand(string demand)
        {
            return _reviewproblemRepositpry.GetReproByDemand(demand);
        }

        /// <summary>
        /// 根据ReviewID删除项目
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int DeleteByReviewId(string ID)
        {
            return _reviewproblemRepositpry.DeleteByReviewID(ID);
        }

        /// <summary>
        /// 需求编号列表分页
        /// </summary>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总条数</param>
        /// <param name="where">搜索条件</param>
        /// <returns></returns>
        public List<CIReviewProblem> GetReproList()
        {
            return _reviewproblemRepositpry.GetReproList();
        }

        /// <summary>
        /// 新增需求
        /// </summary>
        /// <param name="reviewpro"></param>
        /// <returns></returns>
        public int InsertReproWithOutReviewID(CIReviewProblem reviewpro)
        {
            return _reviewproblemRepositpry.InsertReproWithOutDeadLine(reviewpro);
        }

    }
}
