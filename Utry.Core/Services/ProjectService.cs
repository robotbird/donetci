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
    public class ProjectService
    {
        private IProjectRepository _projectRepository;

        #region 构造函数
        /// <summary>
        /// 构造器方法
        /// </summary>
        public ProjectService()
            : this(new ProjectRepository())
        {
        }
        /// <summary>
        /// 构造器方法
        /// </summary>
        /// <param name="userRepository"></param>
        public ProjectService(IProjectRepository projectRepository)
        {
            this._projectRepository = projectRepository;
        }
        #endregion


        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public int InsertPro(CIProject project)
        {
            return _projectRepository.Insert(project);
        }

        /// <summary>
        /// 编辑项目
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public int UpdatePro(CIProject project)
        {
            return _projectRepository.Update(project);
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public int DeletePro(string id)
        {
            var project = _projectRepository.GetProjectByID(id);
            if (project == null)
            {
                return 0;
            }
            int num = _projectRepository.Delete(project);
            return num;
        }

        /// <summary>
        /// 根据项目ID获取项目信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CIProject GetById(string id)
        {
            return _projectRepository.GetById(id);
        }

        /// <summary>
        /// 获取项目列表（分页以及搜索）
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public  IPagedList<CIProject> GetProjectList(int pageSize, int pageIndex, out int recordCount, string where)
        {
            List<CIProject> list;
            try
            {
                list = _projectRepository.GetProjectList(pageSize, pageIndex, out recordCount, where);
                return new PagedList<CIProject>(list, pageIndex -1, pageSize,recordCount);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CIProject> GetProInfoList()
        {
            return _projectRepository.GetProInfoList();
        }

        /// <summary>
        /// 验证项目编号的唯一性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IfExist(string projectcode)
        {
            return _projectRepository.IfExist(projectcode);
        }


    }
}
