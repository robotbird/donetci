using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Utry.Core.Domain;
using Utry.Core.Repositories.IRepository;
using Utry.Core.Repositories.Repository;
using  Utry.Framework.Mvc;

namespace Utry.Core.Services
{
    public class ReportService
    {
        private IReportRepository _reportRepository;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReportService():this(new ReportRepository())
        { 
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReportService(IReportRepository reportRepository)
        {
            this._reportRepository = reportRepository;
        }
        #endregion

        /// <summary>
        /// 根据ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CIReport GetById(string id)
        {
            return _reportRepository.GetById(id);
        }

        /// <summary>
        /// 新增报表
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        public int InsertReport(CIReport report)
        {
            return _reportRepository.Insert(report);
        }

        /// <summary>
        /// 更新报表
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        public int UpdateReport(CIReport report)
        {
            return _reportRepository.Update(report);
        }

        /// <summary>
        /// 删除报表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteReport(string id)
        {
            var report = _reportRepository.GetById(id);
            if (report == null)
            {
                return 0;
            }
            int num = _reportRepository.Delete(report);
            return num;
        }

        /// <summary>
        /// 报表列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public IPagedList<CIReport> GetReportList(int pageSize,int pageIndex,out int recordCount,string where)
        {
            List<CIReport> reportlist;
            try
            {
                reportlist = _reportRepository.GetReportList(pageSize, pageIndex,out recordCount, where);
                return new PagedList<CIReport>(reportlist, pageIndex -1, pageSize,recordCount);
            }
            catch (Exception e)
            {
                    
                throw e;
            }
        }

        /// <summary>
        /// 根据报表SQL语句返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetReportResultBySql(string sql) 
        {
            return _reportRepository.GetReportResultBySql(sql);
        }

    }
}
