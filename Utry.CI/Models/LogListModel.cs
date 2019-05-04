using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utry.Core.Domain;

namespace Utry.CI.Models
{
    public class LogListModel:BaseModel
    {
        public LogListModel()
        {
            Loglist = new List<CISysLog>();
        }


        /// <summary>
        /// 日志名称
        /// </summary>
        public string Logname { get; set; }

        /// <summary>
        /// 日志大小
        /// </summary>
        public string Logsize { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public string Createtime { get; set; }

        /// <summary>
        /// LoglistModel集合
        /// </summary>
        public List<CISysLog> Loglist { get; set; }
    }
    public class CISysLog
    {
        /// <summary>
        /// 日志名称
        /// </summary>
        public string Logname { get; set; }

        /// <summary>
        /// 日志大小
        /// </summary>
        public string Logsize { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public string Createtime { get; set; }
    }
}