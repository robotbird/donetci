using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Utry.Core.Domain;

namespace Utry.Core.Repositories.IRepository
{
    public partial interface IReviewRepository : IRepository<CIReview>
    {
        /// <summary>
        /// 获取所有评审列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<CIReview> GetReviewList(int pageSize, int pageIndex, out int recordCount, string where);

        /// <summary>
        /// 根据项目主键获取评审记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<CIReview> GetReviewListByProjectId(string id, int pageSize, int pageIndex, out int recordCount, string where);
        
    }
}
