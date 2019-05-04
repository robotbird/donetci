using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using Utry.Core.Domain;
using Utry.Framework.Mvc;
namespace Utry.CI.Models
{
    public class ReportModel
    {
        public ReportModel()
        {
            Report = new CIReport();
            ReportList = new List<CIReport>();
            Pagelist = new ReportPageList();
            dtReport = new DataTable();
        }

        public class ReportPageList : BasePageableModel { }

        public CIReport Report { get; set; }

        public List<CIReport> ReportList { get; set; }

        public ReportPageList Pagelist { get; set; }

        public DataTable dtReport { get; set; }

        /// <summary>
        /// 报表名称
        /// </summary>
        public string ReportName { get; set; }


        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndDate { get; set; }
    }
}