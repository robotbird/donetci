using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.VisualStudio.SourceSafe.Interop;
using Utry.Core.Repositories.IRepository;
using Utry.Core.Repositories.Repository;
using Utry.Core.Domain;
using Utry.Framework.Mvc;
using Utry.Framework.Configuration;

namespace Utry.Core.Services
{
   public class LogService
    {
       private ILogRepository _logRepository;

          #region 构造函数
        /// <summary>
        /// 构造器方法
        /// </summary>
        public LogService()
           : this(new LogRepository())
        {
        }
        /// <summary>
        /// 构造器方法
        /// </summary>
        /// <param name="logRepository"></param>
        public LogService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        #endregion

        #region 业务日志
       /// <summary>
       /// 添加日志记录
       /// </summary>
       /// <param name="log"></param>
       /// <returns></returns>
        public int InsertLog(CILog log) 
        {
            log.ID = Guid.NewGuid().ToString();
            return _logRepository.Insert(log);
        }
        #endregion
        #region 系统日志

        /// <summary>
        /// 根据文件名来删除制定Log日志文件
        /// </summary>
        /// <param name="name">指定的Log文件名的物理路径</param>
        /// <returns>执行成功返回1，否则返回0</returns>
        public int DeleteLog(string path)
        {
            FileInfo fileinfo = new FileInfo(path);
            int result = 0;
            if (fileinfo.Exists)
            {
                fileinfo.Delete();
                result = 1;
            }
            else
            {

            }
            return result;
        }
        #endregion


    }
}
