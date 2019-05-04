using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utry.Core.Domain
{
    public class CIReport
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 报表名称
        /// </summary>
        public string ReportName { get; set; }

        /// <summary>
        /// 报表SQL语句
        /// </summary>
        public string ReportSQL { get; set; }

        /// <summary>
        /// 添加日期
        /// </summary>
        public DateTime AddTime { get; set; }
    }
}
