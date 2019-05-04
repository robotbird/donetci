using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SourceSafe.Interop;
using Utry.Core.Repositories.IRepository;
using Utry.Core.Repositories.Repository;
using Utry.Core.Domain;
using Utry.Framework.Mvc;
using Utry.Framework.Configuration;

namespace Utry.Core.Services
{
   public class CheckItemService
    {
       private ICheckItemRepository _checkItemRepository;

       #region 构造函数
        /// <summary>
        /// 构造器方法
        /// </summary>
        public CheckItemService()
           : this(new CheckItemRepository())
        {
        }
        /// <summary>
        /// 构造器方法
        /// </summary>
        /// <param name="itemRepository"></param>
        public CheckItemService(ICheckItemRepository itemRepository)
        {
            this._checkItemRepository = itemRepository;
        }
        #endregion


        #region CICheckItem操作
        /// <summary>
        /// 新增checkitem
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int InsertCheckItem(CICheckItem checkItem)
        {
            //记录日志
            var logService = new LogService();
            var log = new CILog();
            log.Contents = "新增提交物，开发人员：" + checkItem.Developer;
            log.UserName = (new UserService()).GetUserFromCookie().UserName;
            log.LogTime = DateTime.Now;
            log.DemandCode = checkItem.DemandCode;
            log.CodeFile = checkItem.CodeList;
            logService.InsertLog(log);

            return _checkItemRepository.Insert(checkItem);
        }
        /// <summary>
        /// 更新checkitem
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int UpdateCheckItem(CICheckItem checkItem)
        {
            //记录日志
            var logService = new LogService();
            var log = new CILog();
            log.Contents = "修改提交物，开发人员：" + checkItem.Developer;
            log.UserName = (new UserService()).GetUserFromCookie().UserName;
            log.LogTime = DateTime.Now;
            log.DemandCode = checkItem.DemandCode;
            log.CodeFile = checkItem.CodeList;
            logService.InsertLog(log);

            return UpdateCheckItem(checkItem,log);
        }

        /// <summary>
        /// 更新checkitem
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int UpdateCheckItem(CICheckItem checkItem,CILog log)
        {
            if (log != null) 
            {
                new LogService().InsertLog(log);
            }
            return _checkItemRepository.Update(checkItem);
        }
        /// <summary>
        /// 删除checkitem
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int DeleteCheckItem(string Id)
        {
            var checkItem = _checkItemRepository.GetById(Id);
            if (checkItem == null)
            {
                return 0;
            }
            int num = _checkItemRepository.Delete(checkItem);
            if (num > 0) 
            {
                //记录日志
                var logService = new LogService();
                var log = new CILog();
                log.Contents = "删除提交物，开发人员："+checkItem.Developer;
                log.UserName = (new UserService()).GetUserFromCookie().UserName;
                log.LogTime = DateTime.Now;
                log.DemandCode = checkItem.DemandCode;
                log.CodeFile = checkItem.CodeList;
                logService.InsertLog(log);
            }

            return num;
        }
        /// <summary>
        /// 获取单条提交物
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CICheckItem GetCheckItem(string id)
        {
            return _checkItemRepository.GetById(id);
        }
        /// <summary>
        /// 获取提交物列表
        /// </summary>
        /// <returns></returns>
        public IPagedList<CICheckItem> GetCheckItemPageList(int pageSize, int pageIndex, out int recordCount, string where)
        {
            List<CICheckItem> list;
            try
            {
                list = _checkItemRepository.GetCheckList(pageSize, pageIndex, out recordCount, where);
                return new PagedList<CICheckItem>(list, pageIndex - 1, pageSize, recordCount);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 是否存在需求或者bug编号
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool ExistsDemandCode(string code) 
        {
            return _checkItemRepository.ExistsDemandCode(code);
        }
        #endregion

        #region vss操作
        //public VSSDatabaseClass GetVssDatabase() 
        //{
        //    VSSDatabase vssDatabase = new VSSDatabaseClass();
        //    var VSSini = ConfigHelper.GetValue("VSSini");
        //    var VSSuser = ConfigHelper.GetValue("VSSuser");
        //    var VSSpwd = ConfigHelper.GetValue("VSSpwd");
        //    var CCMS_PRJ = ConfigHelper.GetValue("CCMS_PRJ");
        //    var CCMS_Local = ConfigHelper.GetValue("CCMS_Local");

        //    vssDatabase.Open(@VSSini, VSSuser, VSSpwd);
        //    VSSItem vssitem = vssDatabase.get_VSSItem(CCMS_PRJ, false);
        //    return vssDatabase;
        //}

        #endregion

    }
}
