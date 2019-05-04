using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utry.Core.Domain;
using Utry.Framework.Utils;
using Utry.Framework.Web;
using Utry.Framework.Configuration;
using Utry.Core.Repositories.Repository;
using Utry.Core.Repositories.IRepository;

namespace Utry.Core.Services
{
    public class UserOrgService
    {
        private IUserOrgRepository _userorgRepositotry;

        #region 构造函数
        /// <summary>
        /// 构造器方法
        /// </summary>
        public UserOrgService()
            : this(new UserOrgRepository())
        {
        }
        /// <summary>
        /// 构造器方法
        /// </summary>
        /// <param name="userRepository"></param>
        public UserOrgService(IUserOrgRepository userorgRepository)
        {
            this._userorgRepositotry = userorgRepository;
        }
        #endregion

        /// <summary>
        /// 添加小组
        /// </summary>
        /// <returns></returns>
        public int InsertUserOrg(CIUserOrg _userorg)
        {
           return _userorgRepositotry.Insert(_userorg);
        }

        /// <summary>
        /// 修改小组信息
        /// </summary>
        /// <param name="_userorg"></param>
        /// <returns></returns>
        public int UpdateUserOrg(CIUserOrg _userorg)
        {
            return _userorgRepositotry.Update(_userorg);
        }

        /// <summary>
        /// 修改小组信息
        /// </summary>
        /// <param name="_userorg"></param>
        /// <returns></returns>
        public int UpdateUserOrgBystringCode(CIUserOrg _userorg,string OrgCode)
        {
            return _userorgRepositotry.UpdateUserOrg(_userorg, OrgCode);
        }

        /// <summary>
        /// 删除小组信息
        /// </summary>
        /// <param name="_userorg"></param>
        /// <returns></returns>
        public int DeleteUserOrg(string OrgCode)
        {
            var userorg = _userorgRepositotry.GetOrgByCode(OrgCode);
            if (userorg == null)
            {
                return 0;
            }
            int num = _userorgRepositotry.Delete(userorg);

            return num;
        }

        /// <summary>
        /// 根据小组编号获取小组信息
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <returns></returns>
        public CIUserOrg GetOrgByCode(string OrgCode)
        {
            return _userorgRepositotry.GetOrgByCode(OrgCode);
        }

        /// <summary>
        /// 获取全部小组
        /// </summary>
        /// <returns></returns>
        public List<CIUserOrg> GetUserOrgList()
        {
            return _userorgRepositotry.Table.ToList();
        }

        /// <summary>
        /// 判断是否存在小组编号
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <returns></returns>
        public bool ExistUserOrgCode(string OrgCode)
        {
            return _userorgRepositotry.ExistUserOrgCode(OrgCode);
        }
    }
}
