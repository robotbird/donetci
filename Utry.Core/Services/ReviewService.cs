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
    public class ReviewService
    {
        private IReviewRepository _reviewrepository;

        #region "构造函数"
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReviewService()
            : this(new ReviewRepository())
        { 
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="reviewrepository"></param>
        public ReviewService(IReviewRepository reviewrepository)
        {
            this._reviewrepository = reviewrepository;
        }
        #endregion


        /// <summary>
        /// 添加评审
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public int InsertPro(CIReview review)
        {
            return _reviewrepository.Insert(review);
        }

        /// <summary>
        /// 编辑项目
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public int UpdatePro(CIReview review)
        {
            return _reviewrepository.Update(review);
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public int DeletePro(string id)
        {
            var project = _reviewrepository.GetById(id);
            if (project == null)
            {
                return 0;
            }
            int num = _reviewrepository.Delete(project);
            return num;
        }

        /// <summary>
        /// 根据项目ID获取项目信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CIReview GetById(string id)
        {
            return _reviewrepository.GetById(id);
        }

        /// <summary>
        /// 获取项目列表（分页以及搜索）
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public IPagedList<CIReview> GetReviewList(int pageSize, int pageIndex, out int recordCount, string where)
        {
            List<CIReview> list;
            try
            {
                list = _reviewrepository.GetReviewList(pageSize, pageIndex, out recordCount, where);
                return new PagedList<CIReview>(list, pageIndex - 1, pageSize, recordCount);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 根据项目主键获取评审记录
        /// </summary>
        /// <param name="id">项目主键ID</param>
        /// <returns></returns>
        public IPagedList<CIReview> GetReviewListByProjectId(string id,int pageSize, int pageIndex, out int recordCount, string where)
        {
            try
            {
                List<CIReview> list = _reviewrepository.GetReviewListByProjectId(id, pageSize, pageIndex,out recordCount, where);
                return new PagedList<CIReview>(list, pageIndex - 1, pageSize,recordCount);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}
