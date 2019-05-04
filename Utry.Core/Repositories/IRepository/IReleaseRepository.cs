using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Utry.Core.Domain;


namespace Utry.Core.Repositories.IRepository
{
    public interface IReleaseRepository:IRepository<CIRelease>
    {
        /// <summary>
        /// 获取所有的发布列表
        /// </summary>
        List<CIRelease> GetReleaseList(string ProjectID, int pageSize, int pageIndex, out int recordCount, string where);

        /// <summary>
        /// 根据项目编号获取发布成功的下载地址
        /// </summary>
        /// <param name="ProjectCode"></param>
        /// <returns></returns>
        List<CIRelease> GetReleaseListForDownLoad(string ProjectID, int pageSize, int pageIndex, out int recordCount, string where);

        /// <summary>
        /// 根据发布状态获取发布记录
        /// </summary>
        /// <param name="status">发布状态</param>
        /// <returns></returns>
        List<CIRelease> GetReleaseListByStatus(string status);

        /// <summary>
        /// 根据主键ID获取发布日志
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        CIRelease GetLogByID(string ID);

        /// <summary>
        /// 设置测试状态
        /// </summary>
        /// <param name="relrease"></param>
        /// <returns></returns>
        int SetTestStatus(CIRelease release);

    }
}
