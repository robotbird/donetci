using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Utry.Core.Domain;

namespace Utry.Core.Repositories.IRepository
{
    public interface IReportRepository :IRepository<CIReport>
    {
        /// <summary>
        /// 获取所有的报表列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<CIReport> GetReportList(int pageSize, int pageIndex, out int recordCount, string where);

        /// <summary>
        /// 根据报表SQL语句返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataTable GetReportResultBySql(string sql);
    }
}
