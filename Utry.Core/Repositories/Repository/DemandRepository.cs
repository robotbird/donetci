using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Utry.Core.Domain;
using Utry.Core.Repositories.IRepository;
using Utry.Framework.DbProvider;

namespace Utry.Core.Repositories.Repository
{
    public class DemandRepository:IDemandRepository
    {
        /// <summary>
        /// 根据主键获取评审信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CIDemand GetById(object ID)
        {
            var sql = "select * from CIDemand where DemandNumber=@DemandNumber";

            if (ID == null)
            {
                return null;
            }
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Query<CIDemand>(sql, new { DemandNumber = ID.ToString() }).First();
            }
        }




        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int Insert(CIDemand demand)
        {
            var sql = @"INSERT INTO CIReview 
                              (ID,ProjectCode,Purpose,Method,Attachment,Result,Scale,IfReview,PrepareTime,Member,Status,BeginDate,EndDate) 
                      VALUES (@ID,@ProjectCode,@Purpose,@Method,@Attachment,@Result,@Scale,@IfReview,@PrepareTime,@Member,@Status,@BeginDate,@EndDate)";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, demand);
            }
        }

        /// <summary>
        /// 编辑项目
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int Update(CIDemand demand)
        {
            const string sql = @"UPDATE CIDemand SET
				    	                                ProjectCode = @ProjectCode,	
				    	                                Purpose = @Purpose,	
				    	                                Method = @Method,	
				    	                                Attachment = @Attachment,	
				    	                                Result = @Result,	
				    	                                Scale = @Scale,	
				    	                                IfReview = @IfReview,	
				    	                                PrepareTime = @PrepareTime,
                                                        Member = @Member,
                                                        Status = @Status,	
				    	                                BeginDate = @BeginDate,
                                                        EndDate = @EndDate
				    				   WHERE ID=@ID ";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, demand);
            }
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int Delete(CIDemand demand)
        {
            const string sql = "DELETE FROM CIDemand WHERE DemandNumber=@DemandNumber ";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, new { ID = demand.DemandNumber });
            }
        }


        /// <summary>
        /// 获取所有需求编号
        /// </summary>
        public virtual IEnumerable<CIDemand> Table
        {
            get
            {
                string cmdText = string.Format("select * from [V_Demand]");
                using (var conn = new DapperHelper().OpenConnection())
                {
                    var list = conn.Query<CIDemand>(cmdText, null);
                    return list;
                }
            }
        }
    }
}
