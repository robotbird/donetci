using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Utry.Core.Domain;
using Utry.Core.Repositories.IRepository;
using Utry.Framework.DbProvider;

namespace Utry.Core.Repositories.Repository
{
    public class ReleaseRepository:IReleaseRepository
    {
        /// <summary>
        /// 根据主键获取发布信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CIRelease GetById(object ID)
        {
            var sql = "select * from CIRelease where ID=@ID";

            if (ID == null)
            {
                return null;
            }
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Query<CIRelease>(sql, new { ID = ID.ToString() }).First();
            }
        }




        /// <summary>
        /// 添加发布信息
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int Insert(CIRelease release)
        {
            var sql = @"INSERT INTO CIRelease 
                              (ID,ProjectID,Status,Operator,AddTime,Type) 
                      VALUES (@ID,@ProjectID,@Status,@Operator,@AddTime,@Type)";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, release);
            }
        }

        /// <summary>
        /// 编辑发布信息
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int Update(CIRelease release)
        {
            const string sql = @"UPDATE CIRelease SET
                                                        BeginTime = @BeginTime,
                                                        EndTime = @EndTime,
				    	                                LogContent = @LogContent,	
                                                        Status = @Status,
                                                        TestStatus = @TestStatus,
                                                        Operator = @Operator,
                                                        Reversion = @Reversion,
                                                        VersionURL = @VersionURL,
                                                        Remark = @Remark
				    				   WHERE ID=@ID ";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, release);
            }
        }

        /// <summary>
        /// 设置测试状态
        /// </summary>
        /// <param name="relrease"></param>
        /// <returns></returns>
        public int SetTestStatus(CIRelease release)
        {
            const string sql = @"UPDATE CIRelease SET
                                                        TestStatus = @TestStatus
				    				   WHERE ID=@ID ";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, release);
            }
        }

        /// <summary>
        /// 删除发布信息
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int Delete(CIRelease release)
        {
            const string sql = "DELETE FROM CIRelease WHERE ID=@ID ";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, new { ID = release.ID });
            }
        }


        /// <summary>
        /// 获取所有发布信息
        /// </summary>
        public virtual IEnumerable<CIRelease> Table
        {
            get
            {
                string cmdText = string.Format("select * from [CIRelease]");
                using (var conn = new DapperHelper().OpenConnection())
                {
                    var list = conn.Query<CIRelease>(cmdText, null);
                    return list;
                }
            }
        }

        /// <summary>
        /// 根据项目编号获取发布信息列表
        /// </summary>
        /// <param name="ProjectCode">项目编号</param>
        /// <returns></returns>
        public List<CIRelease> GetReleaseList(string ProjectID, int pageSize, int pageIndex, out int recordCount, string where)
        {
            string commandText = @"select * from (select distinct row_number() over (order by addtime desc) as row_num,*,(select
                                    case when datediff(minute, BeginTime, EndTime) < 24*60 then '' else
                                         convert(varchar, datediff(minute, BeginTime, EndTime)/(24*60)) + '天' end +
                                    case when datediff(minute, BeginTime, EndTime) < 60 then '' else
                                         convert(varchar, datediff(minute, BeginTime, EndTime)%(24*60)/60) + '时' end +
                                    convert(varchar, datediff(minute, BeginTime, EndTime)%60) + '分' + 
                                    convert(varchar, datediff(SECOND, BeginTime, EndTime)%60) + '秒')
                                    as spendtime
                                from CIRelease where ProjectID =@ProjectID " + where + @") as tb
                                where row_num between (@pageindex - 1) * @pagesize + 1 and @pageindex * @pagesize";

            var countsql = @"select count(*) as cnt 
                            from [CIRelease]  
                            where ProjectID =@ProjectID " + where;
                                    
            using (var conn = new DapperHelper().OpenConnection())
            {
                var list = conn.Query<CIRelease>(commandText, new { ProjectID = ProjectID, pageindex = pageIndex, pagesize = pageSize });
                recordCount = conn.Query<int>(countsql, new { ProjectID = ProjectID }).First();
                return list.ToList();
            }
        }


        /// <summary>
        /// 根据项目编号获取发布成功的下载地址
        /// </summary>
        /// <param name="ProjectCode"></param>
        /// <returns></returns>
        public List<CIRelease> GetReleaseListForDownLoad(string ProjectID,int pageSize, int pageIndex, out int recordCount, string where)
        {
            string commandText = @"select * from (select distinct row_number() over (order by addtime desc) as row_num,* ,(select
                                    case when datediff(minute, BeginTime, EndTime) < 24*60 then '' else
                                         convert(varchar, datediff(minute, BeginTime, EndTime)/(24*60)) + '天' end +
                                    case when datediff(minute, BeginTime, EndTime) < 60 then '' else
                                         convert(varchar, datediff(minute, BeginTime, EndTime)%(24*60)/60) + '时' end +
                                    convert(varchar, datediff(minute, BeginTime, EndTime)%60) + '分' + 
                                    convert(varchar, datediff(SECOND, BeginTime, EndTime)%60) + '秒')
                                    as spendtime
                                from CIRelease where status ='发布成功' and TestStatus ='测试通过' and Type ='正式版本' and ProjectID =@ProjectID " + where + @") as tb
                                where row_num between (@pageindex - 1) * @pagesize + 1 and @pageindex * @pagesize";
            var countsql = @"select count(*) as cnt 
                            from [CIRelease] 
                            where status ='发布成功' and TestStatus ='测试通过' and Type ='正式版本' and ProjectID =@ProjectID " + where;

            using (var conn = new DapperHelper().OpenConnection())
            {
                var list = conn.Query<CIRelease>(commandText, new { ProjectID = ProjectID, pageindex = pageIndex, pagesize = pageSize });
                recordCount = conn.Query<int>(countsql, new { ProjectID = ProjectID }).First();
                return list.ToList();
            }
        }


        /// <summary>
        /// 根据发布状态获取发布记录
        /// </summary>
        /// <param name="status">发布状态</param>
        /// <returns></returns>
        public List<CIRelease> GetReleaseListByStatus(string status)
        {
            string commandText = @"SELECT * from CIRelease where status =@Status order by AddTime desc";

            using (var conn = new DapperHelper().OpenConnection())
            {
                var list = conn.Query<CIRelease>(commandText, new { Status = status});
                return list.ToList();
            }
        }


        /// <summary>
        /// 根据主键ID获取发布日志
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CIRelease GetLogByID(string ID)
        {
            string commandText = @"SELECT * from CIRelease where ID =@ID";

            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Query<CIRelease>(commandText, new { ID = ID }).First();
            }
        }



    }
}
