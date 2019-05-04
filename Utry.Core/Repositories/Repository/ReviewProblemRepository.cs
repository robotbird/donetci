using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Utry.Core.Repositories.IRepository;
using Utry.Core.Domain;
using Utry.Framework.DbProvider;

namespace Utry.Core.Repositories.Repository
{
    public class ReviewProblemRepository:IReviewProblemRepository
    {
        /// <summary>
        /// 根据主键获取评审信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CIReviewProblem GetById(object ID)
        {
            var sql = "select * from CIReviewProblem where ID=@ID" ;

            if (ID == null)
            {
                return null;
            }
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Query<CIReviewProblem>(sql, new { ID = ID.ToString() }).First();
            }
        }




        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int Insert(CIReviewProblem reviewpro)
        {
            var sql = @"INSERT INTO CIReviewProblem 
                              (ID,ReviewID,DemandCode,Description,Provider,Deadline,Solver,IfSolve,DevelopTime,DesignTime,TestTime,AddTime,UpdateTime) 
                      VALUES (@ID,@ReviewID,@DemandCode,@Description,@Provider,@Deadline,@Solver,@IfSolve,@DevelopTime,@DesignTime,@TestTime,@AddTime,@UpdateTime)";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, reviewpro);
            }
        }

        /// <summary>
        /// 编辑项目
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int Update(CIReviewProblem reviewpro)
        {
            const string sql = @"UPDATE CIReviewProblem SET
				    	                                DemandCode = @DemandCode,	
				    	                                Description = @Description,	
				    	                                Provider = @Provider,	
				    	                                Deadline = @Deadline,	
				    	                                Solver = @Solver,	
				    	                                IfSolve = @IfSolve,
                                                        DevelopTime = @DevelopTime,
                                                        DesignTime = @DesignTime,
                                                        TestTime = @TestTime,
                                                        UpdateTime = @UpdateTime
				    				   WHERE ID=@ID ";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, reviewpro);
            }
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int Delete(CIReviewProblem reviewpro)
        {
            const string sql = "DELETE FROM CIReviewProblem WHERE ID=@ID ";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, new { ID = reviewpro.ID });
            }
        }


        /// <summary>
        /// 根据ReviewID删除项目
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int DeleteByReviewID(string ID)
        {
            const string sql = "DELETE FROM CIReviewProblem WHERE ReviewID=@ReviewID ";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, new { ReviewID = ID });
            }
        }


        /// <summary>
        /// 获取所有项目
        /// </summary>
        public virtual IEnumerable<CIReviewProblem> Table
        {
            get
            {
                string cmdText = string.Format("select * from [CIReviewProblem] order by AddTime desc");
                using (var conn = new DapperHelper().OpenConnection())
                {
                    var list = conn.Query<CIReviewProblem>(cmdText, null);
                    return list;
                }
            }
        }


        public List<CIReviewProblem> GetReviewProByReviewID(string id)
        {
            string cmdText = "select rp.*,v.demandstate from [CIReviewProblem] rp inner join v_demandAndBug v on v.code = rp.DemandCode where ReviewID = @ReviewID order by AddTime desc";
            using (var conn = new DapperHelper().OpenConnection())
            {
                var list = conn.Query<CIReviewProblem>(cmdText, new { ReviewID = id });
                return list.ToList();
            }
        }


        public List<CIReviewProblem> GetReproByDemand(string demand)
        {
            string cmdText = "select * from [CIReviewProblem] where DemandCode = @DemandCode order by AddTime desc";
            using (var conn = new DapperHelper().OpenConnection())
            {
                var list = conn.Query<CIReviewProblem>(cmdText, new { DemandCode = demand });
                return list.ToList();
            }
        }


        /// <summary>
        /// 需求编号列表分页
        /// </summary>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总条数</param>
        /// <param name="where">搜索条件</param>
        /// <returns></returns>
        public List<CIReviewProblem> GetReproList()
        {
            string commandText = @"SELECT * from 
                                    (
                                      select distinct row_number() over (order by rp.Deadline desc) as row_num,rp.*,v.demandstate
                                      from [CIReviewProblem]  rp
                                      inner join v_demandAndBug v on v.code = rp.DemandCode
                                      where 1=1 
                                    ) as tb";

            using (var conn = new DapperHelper().OpenConnection())
            {
                var list = conn.Query<CIReviewProblem>(commandText, null);
                return list.ToList();
            }
                              
        }

        /// <summary>
        /// 新增需求
        /// </summary>
        /// <param name="reviewpro"></param>
        /// <returns></returns>
        public int InsertReproWithOutDeadLine(CIReviewProblem reviewpro)
        {
            var sql = @"INSERT INTO CIReviewProblem 
                              (ID,ReviewID,DemandCode,Description,Provider,Solver,IfSolve,AddTime,UpdateTime) 
                      VALUES (@ID,@ReviewID,@DemandCode,@Description,@Provider,@Solver,@IfSolve,@AddTime,@UpdateTime)";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, reviewpro);
            }
        }





    }
}
