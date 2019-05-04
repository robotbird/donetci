using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Utry.Core.Domain;
using Utry.Core.Repositories.IRepository;
using Utry.Framework.DbProvider;

namespace Utry.Core.Repositories.Repository
{
    public partial class ProjectRepository:IProjectRepository
    {
        /// <summary>
        /// 根据主键获取项目信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CIProject GetById(object ID)
        {
            var sql = "select * from CIProject where ID=@ID";

            if (ID == null)
            {
                return null;
            }
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Query<CIProject>(sql, new { ID = ID.ToString() }).First();
            }
        }

        


        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int Insert(CIProject project)
        {
            var sql = @"INSERT INTO CIProject 
                              (ID,ProjectCode,ProjectName,ProjectManager,Executive,ProjectMember,ProjectFormalURL,ProjectTestURL,DBTestURL,ProjectSvnURL,ProjectSvnURLRelease,AddTime,UpdateTime,Remark,DBDevelopURL,DBFormalURL,SlnName,PackagePath) 
                      VALUES (@ID,@ProjectCode,@ProjectName,@ProjectManager,@Executive,@ProjectMember,@ProjectFormalURL,@ProjectTestURL,@DBTestURL,@ProjectSvnURL,@ProjectSvnURLRelease,@AddTime,@UpdateTime,@Remark,@DBDevelopURL,@DBFormalURL,@SlnName,@PackagePath)";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, project);
            }
        }

        /// <summary>
        /// 编辑项目
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int Update(CIProject project)
        {
            const string sql = @"UPDATE CIProject SET
				    	                                ProjectCode = @ProjectCode,	
				    	                                ProjectName = @ProjectName,	
				    	                                ProjectManager = @ProjectManager,
                                                        Executive = @Executive,	
				    	                                ProjectMember = @ProjectMember,	
                                                        ProjectFormalURL = @ProjectFormalURL,
				    	                                ProjectTestURL = @ProjectTestURL,	
				    	                                DBTestURL = @DBTestURL,	
				    	                                ProjectSvnURL = @ProjectSvnURL,	
                                                        ProjectSvnURLRelease = @ProjectSvnURLRelease,
                                                        UpdateTime = @UpdateTime,
				    	                                Remark = @Remark,
                                                        DBDevelopURL = @DBDevelopURL,
                                                        DBFormalURL = @DBFormalURL,
                                                        SlnName = @SlnName,
                                                        PackagePath = @PackagePath
				    				   WHERE ID=@ID ";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, project);
            }
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="checkitem"></param>
        /// <returns></returns>
        public int Delete(CIProject project)
        {
            const string sql = "DELETE FROM CIProject WHERE ID=@ID ";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(sql, new { ID = project.ID });
            }
        }


        /// <summary>
        /// 获取所有项目
        /// </summary>
        public virtual IEnumerable<CIProject> Table
        {
            get
            {
                string cmdText = string.Format("select * from [CIProject]");
                using (var conn = new DapperHelper().OpenConnection())
                {
                    var list = conn.Query<CIProject>(cmdText, null);
                    return list;
                }
            }
        }


        public List<CIProject> GetProInfoList()
        {
            string cmdText = @"select distinct row_number() over (order by CONVERT(datetime, re.AddTime) desc) as row_num,p.*, CONVERT(datetime, re.AddTime) as begintime
                               from [CIProject]  p
                               left join (
                               select ProjectID, AddTime = (stuff((select top 1 ',' + CONVERT (char(20),AddTime) from CIRelease
                               where ProjectID = a.ProjectID order by AddTime desc for xml path('')),1,1,'')) 
                               from CIRelease a group by ProjectID)
                               re on re.ProjectID = p.ID ";
            using (var conn = new DapperHelper().OpenConnection())
            {
                var list = conn.Query<CIProject>(cmdText, null);
                return list.ToList();
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
        public List<CIProject> GetProjectList(int pageSize, int pageIndex, out int recordCount, string where)
        {
            string commandText = @"SELECT * from 
                                    (
                                      select distinct row_number() over (order by CONVERT(datetime, re.AddTime) desc) as row_num,p.*,r.status, CONVERT(datetime, re.AddTime) as begintime
                                      from [CIProject]  p
                                      left join (select ProjectID, Status = (stuff((select top 1 ',' + Status from CIRelease
                                      where ProjectID = a.ProjectID order by AddTime desc for xml path('')),1,1,'')) from CIRelease a group by ProjectID)
                                      r on r.ProjectID = p.ID
                                      left join (
                                      select ProjectID, AddTime = (stuff((select top 1 ',' + CONVERT (char(20),AddTime) from CIRelease
                                      where ProjectID = a.ProjectID order by AddTime desc for xml path('')),1,1,'')) 
                                      from CIRelease a group by ProjectID)
                                      re on re.ProjectID = p.ID
                                      where 1=1 " + where + @" 
                                    ) as tb
                                   where row_num between (@pageindex - 1) * @pagesize + 1 and @pageindex * @pagesize";
            var countsql = @"select count(*) as cnt 
                            from [CIProject]  p
                            where 1=1 " + where;

            using (var conn = new DapperHelper().OpenConnection())
            {
                var list = conn.Query<CIProject>(commandText, new { pageindex = pageIndex, pagesize = pageSize });
                recordCount = conn.Query<int>(countsql, null).First();
                return list.ToList();
            }
        }


        /// <summary>
        /// 根据主键获取项目信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CIProject GetProjectByID(string ID)
        {
            var sql = "select * from CIProject where ID=@ID";

            if (ID == null)
            {
                return null;
            }
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Query<CIProject>(sql, new { ID = ID.ToString() }).First();
            }
        }

        /// <summary>
        /// 验证项目编号的唯一性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IfExist(string projectcode)
        {

            string cmdText = "select count(*) cnt from CIProject where Projectcode=@Projectcode";
            using (var conn = new DapperHelper().OpenConnection())
            {
                var cnt = conn.Query<int>(cmdText, new { Projectcode = projectcode }).First();
                return cnt > 0;
            }
        }



    }
}
