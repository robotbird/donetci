using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utry.Core.Domain;
using Utry.Core.Repositories.IRepository;
using Utry.Framework.DbProvider;

namespace Utry.Core.Repositories.Repository
{
   public partial class CheckItemRepository:ICheckItemRepository
    {

       public CICheckItem GetById(object Id) 
       {
           var sql = "select * from CICheckItem where Id=@Id";

           if (Id == null) 
           {
               return null;
           }
           using (var conn = new DapperHelper().OpenConnection())
           {
               return conn.Query<CICheckItem>(sql, new { Id = Id.ToString()}).First();
           }
       }
       public int Insert(CICheckItem checkitem) 
       {
           var sql = @"INSERT INTO CICheckItem 
                              (ID,UpdateTime,Status,ValidateNote,DeployNote,VersionCode,DemandCode,CodeList,Attachment,Remark,Developer,Tester,AddTime,GetVssCnt,ItemType,UserName,PlanId) 
                      VALUES (@ID,@UpdateTime,@Status,@ValidateNote,@DeployNote,@VersionCode,@DemandCode,@CodeList,@Attachment,@Remark,@Developer,@Tester,@AddTime,@GetVssCnt,@ItemType,@UserName,@PlanId)";
           using (var conn = new DapperHelper().OpenConnection())
           {
              return conn.Execute(sql, checkitem);
           }
       }
       public int Update(CICheckItem checkitem) 
       {
           const string sql = @"UPDATE CICheckItem SET
				    	                                UpdateTime = @UpdateTime,	
				    	                                Status = @Status,	
				    	                                ValidateNote = @ValidateNote,	
				    	                                DeployNote = @DeployNote,	
				    	                                VersionCode = @VersionCode,	
				    	                                DemandCode = @DemandCode,	
				    	                                CodeList = @CodeList,	
				    	                                Attachment = @Attachment,	
				    	                                Remark = @Remark,	
				    	                                Developer = @Developer,	
				    	                                Tester = @Tester,	
				    	                                AddTime = @AddTime,
				    	                                GetVssCnt = @GetVssCnt,
				    	                                ItemType = @ItemType,
				    	                                UserName = @UserName,
				    	                                PlanId = @PlanId
				    				   WHERE ID=@ID ";
           using (var conn = new DapperHelper().OpenConnection())
           {
               return conn.Execute(sql, checkitem);
           }
       }
       public int Delete(CICheckItem checkitem) 
       {
           const string sql = "DELETE FROM CICheckItem WHERE ID=@ID ";
           using (var conn = new DapperHelper().OpenConnection())
           {
               return conn.Execute(sql, new { ID = checkitem.ID});
           }
       }

       /// <summary>
       /// 获取所有提交物
       /// </summary>
       public virtual IEnumerable<CICheckItem> Table
       {
           get
           {
               string cmdText = string.Format("select * from [CICheckItem]");
               using (var conn = new DapperHelper().OpenConnection())
               {
                   var list = conn.Query<CICheckItem>(cmdText, null);
                   return list;
               }
           }
       }

        /// <summary>
        /// 获取所有的需求和bug列表 分页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="where"></param>
        /// <returns></returns>
       public List<CICheckItem> GetCheckList(int pageSize, int pageIndex, out int recordCount, string where) 
       {
           string commandText = @"SELECT * from 
                                    (
                                      select distinct row_number() over (order by c.updatetime desc) as row_num,c.*,v.demandstate,v.versionnum,p.plancode
                                      from [CICheckItem]  c
                                      inner join v_demandAndBug v on v.code = c.demandcode
                                      left join CIVersionPlan p on p.id = c.planid
                                      left join CIUser u on u.username = c.username
                                      where 1=1 " + where + @" 
                                    ) as tb
                                   where row_num between (@pageindex - 1) * @pagesize + 1 and @pageindex * @pagesize";
           var countsql = @"select count(*) as cnt 
                            from [CICheckItem]  c
                            inner join v_demandAndBug v on v.code = c.demandcode
                            left join CIVersionPlan p on p.id = c.planid 
                            left join CIUser u on u.username = c.username
                            where 1=1 " + where;

           using (var conn = new DapperHelper().OpenConnection())
           {
               var list = conn.Query<CICheckItem>(commandText, new { pageindex = pageIndex, pagesize = pageSize });
               recordCount = conn.Query<int>(countsql, null).First();
               return list.ToList();
           }
       }
       /// <summary>
       /// 是否存在这个编号
       /// </summary>
       /// <param name="code"></param>
       /// <returns></returns>
       public bool ExistsDemandCode(string code)
       {
           string cmdText = @" select count(*) as cnt from v_demandAndBug
                               where code=@code ";
           using (var conn = new DapperHelper().OpenConnection())
           {
               var cnt = conn.Query<int>(cmdText, new { code = code }).First();
               return cnt > 0;
           }
       }
    }
}
