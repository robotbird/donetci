using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Utry.Core.Domain;
using Utry.Framework.Utils;
using Utry.Framework.Web;
using Utry.Framework.Configuration;
using Utry.Core.Repositories.Repository;
using Utry.Core.Repositories.IRepository;
using Utry.Framework.Mvc;

namespace Utry.Core.Services
{
    public class ReleaseService
    {
        private IReleaseRepository _releaseRepository;

        #region 构造函数
        /// <summary>
        /// 构造器方法
        /// </summary>
        public ReleaseService()
            : this(new ReleaseRepository())
        {
        }
        /// <summary>
        /// 构造器方法
        /// </summary>
        /// <param name="userRepository"></param>
        public ReleaseService(IReleaseRepository releaseRepository)
        {
            this._releaseRepository = releaseRepository;
        }
        #endregion

        /// <summary>
        /// 添加发布信息
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public int InsertPro(CIRelease release)
        {
            return _releaseRepository.Insert(release);
        }

        /// <summary>
        /// 编辑发布信息
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public int UpdatePro(CIRelease release)
        {
            return _releaseRepository.Update(release);
        }

        /// <summary>
        /// 删除发布信息
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public int DeletePro(string id)
        {
            var project = _releaseRepository.GetById(id);
            if (project == null)
            {
                return 0;
            }
            int num = _releaseRepository.Delete(project);
            return num;
        }

        /// <summary>
        /// 根据发布ID获取发布信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CIRelease GetById(string id)
        {
            return _releaseRepository.GetById(id);
        }

        /// <summary>
        /// 根据项目编号获取发布信息列表
        /// </summary>
        /// <param name="ProjectCode">项目编号</param>
        /// <returns></returns>
        public IPagedList<CIRelease> GetReleaseList(string ProjectCode, int pageSize, int pageIndex, out int recordCount, string where)
        {
            List<CIRelease> list;
            try
            {
                list = _releaseRepository.GetReleaseList(ProjectCode,pageSize, pageIndex, out recordCount, where);
                return new PagedList<CIRelease>(list, pageIndex - 1, pageSize, recordCount);
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        /// <summary>
        /// 根据项目编号获取发布成功的下载地址
        /// </summary>
        /// <param name="ProjectCode"></param>
        /// <returns></returns>
        public IPagedList<CIRelease> GetReleaseListForDownLoad(string ProjectCode, int pageSize, int pageIndex, out int recordCount, string where)
        {
            List<CIRelease> list;
            try
            {
                list = _releaseRepository.GetReleaseListForDownLoad(ProjectCode, pageSize, pageIndex, out recordCount, where);
                return new PagedList<CIRelease>(list, pageIndex - 1, pageSize, recordCount);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 根据发布状态获取发布记录
        /// </summary>
        /// <param name="status">发布状态</param>
        /// <returns></returns>
       public List<CIRelease> GetReleaseListByStatus(string status)
        {
            return _releaseRepository.GetReleaseListByStatus(status);
        }

       public CIRelease GetLogByID(string ID)
       {
           return _releaseRepository.GetLogByID(ID);
       }

       /// <summary>
       /// 设置测试状态
       /// </summary>
       /// <param name="relrease"></param>
       /// <returns></returns>
       public int SetTestStatus(CIRelease release)
       {
           return _releaseRepository.SetTestStatus(release);
       }









    }
}
