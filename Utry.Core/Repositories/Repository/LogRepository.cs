using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utry.Core.Domain;
using Utry.Core.Repositories.IRepository;
using Utry.Framework.DbProvider;


namespace Utry.Core.Repositories.Repository
{
    public partial class LogRepository:ILogRepository
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public int Insert(CILog log) 
        {
            string cmdText = @"INSERT INTO CILog (ID,LogTime,Contents,UserName,DemandCode,CodeFile)
                                          VALUES (@ID,@LogTime,@Contents,@UserName,@DemandCode,@CodeFile)";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(cmdText, log);
            }
        }
        public int Update(CILog log) 
        {
            var sql = @"UPDATE CILog SET
				    	                                LogTime = @LogTime,	
				    	                                Contents = @Contents,	
				    	                                UserName = @UserName,	
				    	                                DemandCode = @DemandCode,	
				    	                                CodeFile = @CodeFile	
				    				   WHERE ID=@ID";
            return 0;
        }
        public int Delete(CILog log) 
        {
            return 0;
        }
        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CILog GetById(object Id) 
        {
            string cmdText = "select * from CILog where ID=@Id ";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Query<CILog>(cmdText, new { userName = Id.ToString() }).First();
            }
        }
        /// <summary>
        /// 获取全部日志
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<CILog> Table
        {
            get
            {
                string cmdText = "select * from CILog";
                using (var conn = new DapperHelper().OpenConnection())
                {
                    var list = conn.Query<CILog>(cmdText, null);
                    return list;
                }
            }
        }
    }
}
