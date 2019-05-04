using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Utry.Core.Domain;

namespace Utry.Core.Repositories.IRepository
{
    public partial interface IProjectRepository : IRepository<CIProject>
    {
        /// <summary>
        /// 获取所有的项目列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<CIProject> GetProjectList(int pageSize, int pageIndex, out int recordCount, string where);

        /// <summary>
        /// 根据主键获取项目信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        CIProject GetProjectByID(string ID);


        /// <summary>
        /// 获取项目信息列表
        /// </summary>
        /// <returns></returns>
        List<CIProject> GetProInfoList();

        /// <summary>
        /// 验证项目编号的唯一性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IfExist(string projectcode);
    }
}
