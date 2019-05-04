using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utry.Core.Domain;

namespace Utry.Core.Repositories.IRepository
{
    public partial interface IUserOrgRepository:IRepository<CIUserOrg>
    {

        /// <summary>
        /// 判断是否存在该小组编号
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <returns></returns>
        bool ExistUserOrgCode(string OrgCode);

        /// <summary>
        /// 根据小组编号获取小组信息
        /// </summary>
        /// <param name="Orgcode"></param>
        /// <returns></returns>
        CIUserOrg GetOrgByCode(string Orgcode);

        /// <summary>
        /// 更新小组信息
        /// </summary>
        /// <param name="userorg"></param>
        /// <param name="orgcode"></param>
        /// <returns></returns>
        int UpdateUserOrg(CIUserOrg userorg, string orgcode);
    }
}
