using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utry.Core.Domain;
namespace Utry.Core.Repositories.IRepository
{
    public partial interface IReviewProblemRepository : IRepository<CIReviewProblem>
    {
        /// <summary>
        /// 根据评审ID获取对应问题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<CIReviewProblem> GetReviewProByReviewID(string id);

        /// <summary>
        /// 根据需求编号获取问题
        /// </summary>
        /// <param name="demand"></param>
        /// <returns></returns>
        List<CIReviewProblem> GetReproByDemand(string demand);

        /// <summary>
        /// 需求编号列表分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="?"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<CIReviewProblem> GetReproList();

        int DeleteByReviewID(string ID);

        /// <summary>
        /// 新增需求
        /// </summary>
        /// <param name="reviewpro"></param>
        /// <returns></returns>
        int InsertReproWithOutDeadLine(CIReviewProblem reviewpro);

    }
}
