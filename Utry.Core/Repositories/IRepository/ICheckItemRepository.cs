using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utry.Core.Domain;

namespace Utry.Core.Repositories.IRepository
{
    public partial interface ICheckItemRepository : IRepository<CICheckItem>
    {
        /// <summary>
        /// 获取所有的需求和bug列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<CICheckItem> GetCheckList(int pageSize, int pageIndex, out int recordCount, string where);
        /// <summary>
        /// 检查需求和bug编号是否存在
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        bool ExistsDemandCode(string code);
    }
}
