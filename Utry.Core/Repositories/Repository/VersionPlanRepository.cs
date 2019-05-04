using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utry.Core.Domain;
using Utry.Core.Repositories.IRepository;
using Utry.Framework.DbProvider;

namespace Utry.Core.Repositories.Repository
{
    public partial class VersionPlanRepository:IVersionPlanRepository
    {
        /// <summary>
        /// 获取计划
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CIVersionPlan GetById(object Id)
        {
            var sql = "select * from CIVersionPlan where Id=@Id";
            if (Id == null)
            {
                return null;
            }
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Query<CIVersionPlan>(sql, new { Id = Id.ToString() }).First();
            }
        }
        /// <summary>
        /// 新增一个计划
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public int Insert(CIVersionPlan plan)
        {
            var sql = @"INSERT INTO CIVersionPlan (ID,BeginTime,EndTime,PlanCode,Status,Note,AddTime,UpdateTime,UserName,OpenDate)
                                          VALUES (@ID,@BeginTime,@EndTime,@PlanCode,@Status,@Note,@AddTime,@UpdateTime,@UserName,@OpenDate)";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, plan);
            }
        }
        /// <summary>
        /// 更新计划
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public int Update(CIVersionPlan plan)
        {
            const string sql = @"UPDATE CIVersionPlan SET
				    	                                BeginTime = @BeginTime,	
				    	                                EndTime = @EndTime,	
				    	                                PlanCode = @PlanCode,	
				    	                                Status = @Status,	
				    	                                Note = @Note,	
				    	                                UpdateTime = @UpdateTime,	
				    	                                UserName = @UserName,
				    	                                OpenDate = @OpenDate
				    				   WHERE ID=@ID";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, plan);
            }
        }
        /// <summary>
        /// 删除计划
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public int Delete(CIVersionPlan plan)
        {
            const string sql = "DELETE FROM CIVersionPlan WHERE ID=@ID ";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, new { ID = plan.ID });
            }
        }
        /// <summary>
        /// 获取所有计划
        /// </summary>
        public virtual IEnumerable<CIVersionPlan> Table
        {
            get
            {
                string cmdText = string.Format("select * from [CIVersionPlan] order by updatetime desc");
                using (var conn = new DapperHelper().OpenConnection())
                {
                    var list = conn.Query<CIVersionPlan>(cmdText, null);
                    return list;
                }
            }
        }
        /// <summary>
        /// 获取计划分页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<CIVersionPlan> GetPlanList(int pageSize, int pageIndex, out int recordCount, string where)
        {
            string commandText = @"SELECT * from 
                                    (
                                      select distinct row_number() over (order by updatetime desc) as row_num, c.*
                                      FROM [CIVersionPlan] c where 1=1 " + where + @"  
                                    ) as tb
                                   where row_num between (@pageindex - 1) * @pagesize + 1 and @pageindex * @pagesize";
            var countsql = @"select count(*) as cnt from CIVersionPlan where 1=1 "+where;

            using (var conn = new DapperHelper().OpenConnection())
            {
                var list = conn.Query<CIVersionPlan>(commandText, new { pageindex = pageIndex, pagesize = pageSize });
                recordCount = conn.Query<int>(countsql,null).First();
                return list.ToList();
            }
        }
    }
}
