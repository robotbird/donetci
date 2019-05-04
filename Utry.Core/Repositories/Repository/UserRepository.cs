using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utry.Core.Domain;
using Utry.Core.Repositories.IRepository;
using Utry.Framework.DbProvider;

namespace Utry.Core.Repositories.Repository
{
    public partial class UserRepository : IUserRepository
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="CIUser"></param>
        /// <returns></returns>
        public int Insert(CIUser CIUser)
        {
            string cmdText = @"INSERT INTO CIUser (UserName,Email,UTMPName,Mobile,Role,PassWord,Status,RegTime,FullName,OrgCode) VALUES
                                                  (@UserName,@Email,@UTMPName,@Mobile,@Role,@PassWord,@Status,@RegTime,@FullName,@OrgCode)";
            using (var conn = new DapperHelper().OpenConnection())
            {
               return conn.Execute(cmdText, CIUser);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="CIUser"></param>
        /// <returns></returns>
        public int Update(CIUser CIUser)
        {
            string cmdText = @"UPDATE CIUser SET
				    	                    Email = @Email,	
				    	                    UTMPName = @UTMPName,	
				    	                    Mobile = @Mobile,	
				    	                    Role = @Role,	
				    	                    PassWord = @PassWord,	
				    	                    FullName = @FullName,	
				    	                    Status = @Status,
                                            OrgCode = @OrgCode	
				    		WHERE UserName=@UserName";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(cmdText, CIUser);
            }
        }

        /// <summary>
        /// 更新开发人员所属小组
        /// </summary>
        /// <param name="CIUser"></param>
        /// <returns></returns>
        public int UpdateUserOrg(CIUser CIUser)
        {
            string cmdText = @"UPDATE CIUser SET
                                            Role = @Role,
                                            PassWord = @PassWord,
                                            FullName = @FullName,
                                            OrgCode = @OrgCode
				    		WHERE UserName=@UserName";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(cmdText, CIUser);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int Delete(CIUser CIUser)
        {
            string cmdText = "delete from CIUser where UserName=@UserName";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Execute(cmdText, new { UserName = CIUser.UserName });
            }
        }


        /// <summary>
        /// 获取用户 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CIUser GetById(object userName)
        {
            string cmdText = "select * from CIUser where userName = @userName ";
            using (var conn = new DapperHelper().OpenConnection())
            {
                var list = conn.Query<CIUser>(cmdText, new { userName = userName.ToString() });
                if (list != null && list.Count() > 0)
                {
                    return list.First();
                }
                else 
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 根据用户名获取用户 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public CIUser GetUserByName(string userName)
        {
            string cmdText = "select * from CIUser where [userName] = @userName ";
            using (var conn = new DapperHelper().OpenConnection())
            {
                return conn.Query<CIUser>(cmdText, new { userName = userName }).First();
            }
        }

        /// <summary>
        /// 根据用户名和密码获取用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public CIUser GetUser(string userName, string password)
        {
            string cmdText = "select * from CIUser where [userName] = @userName and [password]=@password ";
            using (var conn = new DapperHelper().OpenConnection())
            {
                var list = conn.Query<CIUser>(cmdText, new { userName = userName, password = password });
                return list.Count() == 0 ? null : list.First();
            }
        }

        /// <summary>
        /// 获取全部用户
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<CIUser> Table
        {
            get
            {
                string cmdText = "select * from CIUser";
                using (var conn = new DapperHelper().OpenConnection())
                {
                    var list = conn.Query<CIUser>(cmdText, null);
                    return list;
                }
            }
        }

        /// <summary>
        /// 获取包括小组信息的全部人员信息
        /// </summary>
        /// <returns></returns>
        public List<CIUser> GetAllUserInfo()
        {
           string cmdText = "select u.*,uo.* from CIUser u left join CIUserOrg uo on u.OrgCode = uo.OrgCode order by RegTime desc";
           using (var conn = new DapperHelper().OpenConnection())
           {
               var list = conn.Query<CIUser>(cmdText, null);
               return list.ToList();
           }
        }



        /// <summary>
        /// 是否存在此用户名
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool ExistsUserName(string userName)
        {
            string cmdText = "SELECT COUNT(*) cnt FROM CIUser  WHERE  userName = @UserName ";
            using (var conn = new DapperHelper().OpenConnection())
            {
                var cnt = conn.Query<int>(cmdText, new { UserName = userName }).First();
                return  cnt> 0;
            }
        }

        /// <summary>
        /// 根据全名获取用户 
        /// </summary>
        /// <returns></returns>
        public List<CIUser> GetUserByFullName(string fullname)
        {
            string cmdText = "select * from CIUser where FullName like '%" + fullname + "%' order by RegTime desc";
            using (var conn = new DapperHelper().OpenConnection())
            {
                var list = conn.Query<CIUser>(cmdText, null);
                return list.ToList();
            }
        }
    }
}
