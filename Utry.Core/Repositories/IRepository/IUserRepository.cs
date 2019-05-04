using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utry.Core.Domain;

namespace Utry.Core.Repositories.IRepository
{
    public partial interface IUserRepository : IRepository<CIUser>
    {
        /// <summary>
        /// 是否存在此用户名
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool ExistsUserName(string userName);

        /// <summary>
        /// 根据用户名获取用户 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        CIUser GetUserByName(string userName);

        /// <summary>
        /// 根据用户名和密码获取用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        CIUser GetUser(string userName, string password);

        /// <summary>
        /// 获取包裹小组信息的全部人员信息
        /// </summary>
        /// <returns></returns>
        List<CIUser> GetAllUserInfo();

        /// <summary>
        /// 更新开发人员所属小组
        /// </summary>
        /// <param name="CIUser"></param>
        /// <returns></returns>
        int UpdateUserOrg(CIUser CIUser);

        /// <summary>
        /// 根据全名获取用户 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        List<CIUser> GetUserByFullName(string fullName);
    }
}
