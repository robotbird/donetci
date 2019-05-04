using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utry.Core.Domain;
using Utry.Core.Repositories.IRepository;
using Utry.Framework.DbProvider;

namespace Utry.Core.Repositories.Repository
{
    public class UserOrgRepository: IUserOrgRepository
    {

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="CIUserOrg"></param>
        /// <returns></returns>
        public int Insert(CIUserOrg CIUserOrg)
        {
            string cmdText = @"INSERT INTO CIUserOrg (OrgName,OrgCode,ParentCode) VALUES
                                                  (@OrgName,@OrgCode,@ParentCode)";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(cmdText, CIUserOrg);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="CIUserOrg"></param>
        /// <returns></returns>
        public int Update(CIUserOrg CIUserOrg)
        {
            string cmdText = @"UPDATE CIUserOrg SET 
                                       OrgName = @OrgName ,
                                       OrgCode = @OrgCode, 
                                       ParentCode = @ParentCode
                                   WHERE OrgCode = @OrgCode";
            using( var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(cmdText, CIUserOrg);
            }
        }


        /// <summary>
        /// 更新小组信息
        /// </summary>
        /// <param name="CIUserOrg"></param>
        /// <returns></returns>
        public int UpdateUserOrg(CIUserOrg userorg, string orgcode)
        {
            string cmdText = @"UPDATE CIUserOrg SET 
                                       OrgName = @OrgName ,
                                       OrgCode = @OrgCode, 
                                       ParentCode = @ParentCode
                                   WHERE OrgCode = '"+ orgcode +"'";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(cmdText, userorg);
            }
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CIUserOrg"></param>
        /// <returns></returns>
        public int Delete(CIUserOrg CIUserOrg)
        {
            string cmdText = "DELETE FROM CIUserOrg WHERE OrgCode = @OrgCode";
            using ( var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(cmdText, CIUserOrg);
            }
        }

        /// <summary>
        /// 获取小组
        /// </summary>
        /// <returns></returns>
        public CIUserOrg GetById(object OrgCode)
        {
            string cmdText = "SELECT * FROM CIUserOrg WHERE OrgCode = @OrgCode";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Query<CIUserOrg>(cmdText, new { OrgCode = OrgCode.ToString() }).First();
            }
        }
        
        /// <summary>
        /// 根据小组编号获取小组信息
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <returns></returns>
        public CIUserOrg GetOrgByCode(string OrgCode)
        {
            string cmdText = "SELECT * FROM CIUserOrg WHERE OrgCode = @OrgCode";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Query<CIUserOrg>(cmdText, new { OrgCode = OrgCode }).First();
            }
        }

        /// <summary>
        /// 获取全部小组
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<CIUserOrg> Table
        {
            get
            {
                string cmdText = "select * from CIUserOrg";
                using (var conn = new DapperHelper().OpenConnection())
                {
                    var list = conn.Query<CIUserOrg>(cmdText, null);
                    return list;
                }
            }
        }

        /// <summary>
        /// 判断是否存在该小组编号
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <returns></returns>
        public bool ExistUserOrgCode(string OrgCode)
        {
            string cmdText = "select count(*) cnt from CIUserOrg where OrgCode = @OrgCode";
            using (var conn = new DapperHelper().OpenConnection())
            {
                var cnt = conn.Query<int>(cmdText, new { OrgCode=OrgCode }).First();
                return cnt > 0;
            }
        }

    }
}
