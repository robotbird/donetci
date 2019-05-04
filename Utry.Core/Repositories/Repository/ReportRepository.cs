using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Utry.Core.Domain;
using Utry.Core.Repositories.IRepository;
using Utry.Framework.DbProvider;

namespace Utry.Core.Repositories.Repository
{
    public class ReportRepository : IReportRepository
    {
        /// <summary>
        /// 根据主键获取报表信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CIReport GetById(object ID)
        {
            var sql = "select * from CIReport where ID=@ID";

            if (ID == null)
            {
                return null;
            }
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Query<CIReport>(sql, new { ID = ID.ToString() }).First();
            }
        }




        /// <summary>
        /// 添加报表
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int Insert(CIReport report)
        {
            var sql = @"INSERT INTO CIReport 
                              (ID,ReportName,ReportSQL,AddTime) 
                      VALUES (@ID,@ReportName,@ReportSQL,@AddTime)";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, report);
            }
        }

        /// <summary>
        /// 编辑报表
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int Update(CIReport report)
        {
            const string sql = @"UPDATE CIReport SET
				    	                                ReportName = @ReportName,	
				    	                                ReportSQL = @ReportSQL	
				    				   WHERE ID=@ID ";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, report);
            }
        }

        /// <summary>
        /// 删除报表
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int Delete(CIReport report)
        {
            const string sql = "DELETE FROM CIReport WHERE ID=@ID ";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, new { ID = report.ID });
            }
        }


        /// <summary>
        /// 获取所有报表
        /// </summary>
        public virtual IEnumerable<CIReport> Table
        {
            get
            {
                string cmdText = string.Format("select * from [CIReport]");
                using (var conn = new DapperHelper().OpenConnection())
                {
                    var list = conn.Query<CIReport>(cmdText, null);
                    return list;
                }
            }
        }


        /// <summary>
        /// 获取所有的报表列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<CIReport> GetReportList(int pageSize, int pageIndex, out int recordCount, string where)
        {
            string commandText = @"SELECT * from 
                                    (
                                      select distinct row_number() over (order by r.addtime desc) as row_num,r.*
                                      from [CIReport]  r
                                      where 1=1 " + where + @" 
                                    ) as tb
                                   where row_num between (@pageindex - 1) * @pagesize + 1 and @pageindex * @pagesize";
            var countsql = @"select count(*) as cnt 
                            from [CIReport]  r
                            where 1=1 " + where;

            using (var conn = new DapperHelper().OpenConnection())
            {
                var list = conn.Query<CIReport>(commandText, new { pageindex = pageIndex, pagesize = pageSize });
                recordCount = conn.Query<int>(countsql, null).First();
                return list.ToList();
            }
        }

        /// <summary>
        /// 根据报表SQL语句返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetReportResultBySql(string sql) 
        {
            var dt = DbHelper.ExecuteDataset(sql).Tables[0];
            return dt;
        }
    }
}
