using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Utry.Core.Repositories.IRepository;
using Utry.Core.Domain;
using Utry.Framework.DbProvider;

namespace Utry.Core.Repositories.Repository
{
    public class ReviewRepository:IReviewRepository
    {
        /// <summary>
        /// 根据主键获取评审信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CIReview GetById(object ID)
        {
            var sql = "select r.*,p.ID as ProjectID from CIReview r inner join CIProject p on r.ProjectCode = p.ProjectCode where r.ID=@ID";

            if (ID == null)
            {
                return null;
            }
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Query<CIReview>(sql, new { ID = ID.ToString() }).First();
            }
        }




        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int Insert(CIReview review)
        {
            var sql = @"INSERT INTO CIReview 
                              (ID,ProjectCode,Purpose,Method,Attachment,Result,Scale,IfReview,PrepareTime,Member,Status,BeginDate,EndDate,AddTime,UpdateTime) 
                      VALUES (@ID,@ProjectCode,@Purpose,@Method,@Attachment,@Result,@Scale,@IfReview,@PrepareTime,@Member,@Status,@BeginDate,@EndDate,@AddTime,@UpdateTime)";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, review);
            }
        }

        /// <summary>
        /// 编辑项目
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int Update(CIReview review)
        {
            const string sql = @"UPDATE CIReview SET
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
                                                        EndDate = @EndDate,
                                                        UpdateTime = @UpdateTime
				    				   WHERE ID=@ID ";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, review);
            }
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int Delete(CIReview review)
        {
            const string sql = "DELETE FROM CIReview WHERE ID=@ID ";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, new { ID = review.ID });
            }
        }


        /// <summary>
        /// 获取所有项目
        /// </summary>
        public virtual IEnumerable<CIReview> Table
        {
            get
            {
                string cmdText = string.Format("select * from [CIReview]");
                using (var conn = new DapperHelper().OpenConnection())
                {
                    var list = conn.Query<CIReview>(cmdText, null);
                    return list;
                }
            }
        }




        /// <summary>
        /// 获取所有的项目列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<CIReview> GetReviewList(int pageSize, int pageIndex, out int recordCount, string where)
        {
            string commandText = @"SELECT * from 
                                    (
                                      select distinct row_number() over (order by r.BeginDate desc) as row_num,r.*,p.ProjectName,rp.DemandCode,rp1.Description
                                      from [CIReview]  r
                                      left join CIProject p on r.ProjectCode = p.ProjectCode
                                      left join (select ReviewID, DemandCode = (stuff((select ',' + DemandCode from CIReviewProblem where ReviewID =   
                                      a.ReviewID for xml path('')),1,1,'')) from CIReviewProblem a group by ReviewID) rp on r.ID = rp.ReviewID
                                      left join (select ReviewID, Description = (stuff((select ',' + Description from CIReviewProblem where ReviewID = a.ReviewID 
                                      for xml path('')),1,1,'')) from CIReviewProblem a group by ReviewID) rp1 on r.ID = rp1.ReviewID
                                      where 1=1 " + where + @" 
                                    ) as tb
                                   where row_num between (@pageindex - 1) * @pagesize + 1 and @pageindex * @pagesize";
            var countsql = @"select count(*) as cnt 
                            from [CIReview]  r
                            left join CIProject p on r.ProjectCode = p.ProjectCode
                            left join (select ReviewID, DemandCode = (stuff((select ',' + DemandCode from CIReviewProblem where ReviewID =   
                            a.ReviewID for xml path('')),1,1,'')) from CIReviewProblem a group by ReviewID) rp on r.ID = rp.ReviewID
                            left join (select ReviewID, Description = (stuff((select ',' + Description from CIReviewProblem where ReviewID = a.ReviewID 
                            for xml path('')),1,1,'')) from CIReviewProblem a group by ReviewID) rp1 on r.ID = rp1.ReviewID
                            where 1=1 " + where;

            using (var conn = new DapperHelper().OpenConnection())
            {
                var list = conn.Query<CIReview>(commandText, new { pageindex = pageIndex, pagesize = pageSize });
                recordCount = conn.Query<int>(countsql, null).First();
                return list.ToList();
            }
        }


        /// <summary>
        /// 根据项目主键获取评审记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CIReview> GetReviewListByProjectId(string id, int pageSize, int pageIndex, out int recordCount, string where)
        {
            string cmdText = @"SELECT * from (select distinct row_number() over (order by r.BeginDate desc) as row_num,r.ID,r.BeginDate,r.Status,rp.DemandCode,r.EndDate,rp1.Description,p.ID as ProjectID from CIReview r 
                              left join CIProject p on r.ProjectCode=p.ProjectCode
                              left join (select ReviewID, DemandCode = (stuff((select ',' + DemandCode from CIReviewProblem where ReviewID = a.ReviewID 
                              for xml path('')),1,1,'')) from CIReviewProblem a group by ReviewID) rp on r.ID = rp.ReviewID 
                              left join (select ReviewID, Description = (stuff((select ',' + Description from CIReviewProblem where ReviewID = a.ReviewID 
                              for xml path('')),1,1,'')) from CIReviewProblem a group by ReviewID) rp1 on r.ID = rp1.ReviewID
                              where p.ID=@ID " + where + ") as tb where row_num between (@pageindex - 1) * @pagesize + 1 and @pageindex * @pagesize";

            var countsql = @"select count(*) as cnt from CIReview r 
                              left join CIProject p on r.ProjectCode=p.ProjectCode
                              left join (select ReviewID, DemandCode = (stuff((select ',' + DemandCode from CIReviewProblem where ReviewID = a.ReviewID 
                              for xml path('')),1,1,'')) from CIReviewProblem a group by ReviewID) rp on r.ID = rp.ReviewID 
                              left join (select ReviewID, Description = (stuff((select ',' + Description from CIReviewProblem where ReviewID = a.ReviewID 
                              for xml path('')),1,1,'')) from CIReviewProblem a group by ReviewID) rp1 on r.ID = rp1.ReviewID
                              where p.ID=@ID " + where ;

            using (var conn = new DapperHelper().OpenConnection())
            {
                var list = conn.Query<CIReview>(cmdText, new { ID = id, pageindex = pageIndex, pagesize = pageSize });
                recordCount = conn.Query<int>(countsql, new { ID = id }).First();
                return list.ToList();
            }
        }

    }
}
