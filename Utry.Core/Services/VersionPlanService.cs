using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utry.Core.Repositories.IRepository;
using Utry.Core.Repositories.Repository;
using Utry.Core.Domain;
using Utry.Framework.Mvc;
using Utry.Framework.Configuration;

namespace Utry.Core.Services
{
   public  class VersionPlanService
    {
       private IVersionPlanRepository _planRepository;
        #region 构造函数
        /// <summary>
        /// 构造器方法
        /// </summary>
        public VersionPlanService()
           : this(new VersionPlanRepository())
        {
        }
        /// <summary>
        /// 构造器方法
        /// </summary>
        /// <param name="planRepository"></param>
        public VersionPlanService(IVersionPlanRepository planRepository)
        {
            this._planRepository = planRepository;
        }
        #endregion

       /// <summary>
       /// 新增计划
       /// </summary>
       /// <param name="plan"></param>
       /// <returns></returns>
        public int InsertVersionPlan(CIVersionPlan plan) 
        {
            return _planRepository.Insert(plan);
        }
       /// <summary>
       /// 更新计划
       /// </summary>
       /// <param name="plan"></param>
       /// <returns></returns>
        public int UpdateVersionPlan(CIVersionPlan plan) 
        {
            return _planRepository.Update(plan);
        }
       /// <summary>
       /// 删除计划
       /// </summary>
       /// <param name="Id"></param>
       /// <returns></returns>
        public int DeleteVersionPlan(string Id) 
        {
            return _planRepository.Delete(new CIVersionPlan() {ID = Id });
        }
       /// <summary>
       /// 获取计划
       /// </summary>
       /// <param name="Id"></param>
       /// <returns></returns>
        public CIVersionPlan GetPlan(string Id) 
        {
            return _planRepository.GetById(Id);
        }
       /// <summary>
       /// 获取所有计划
       /// </summary>
       /// <returns></returns>
        public List<CIVersionPlan> GetPlanList()
        {
            return _planRepository.Table.ToList();
        }
       /// <summary>
       /// 获取计划分页
       /// </summary>
       /// <param name="pageSize"></param>
       /// <param name="pageIndex"></param>
       /// <param name="recordCount"></param>
       /// <param name="where"></param>
       /// <returns></returns>
        public IPagedList<CIVersionPlan> GetPlanPageList(int pageSize, int pageIndex, out int recordCount, string where)
        {
            List<CIVersionPlan> list;
            try
            {
                list = _planRepository.GetPlanList(pageSize, pageIndex, out recordCount, where);
                return new PagedList<CIVersionPlan>(list, pageIndex - 1, pageSize, recordCount);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
