using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utry.Core.Domain
{
    public class CILog
    {

        /// <summary>
        /// ID
        /// </summary>		
        public string ID { get; set; }
        /// <summary>
        /// 日志类型，暂时没用
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 日志记录时间
        /// </summary>		
        public DateTime LogTime { get; set; }
        /// <summary>
        /// 操作内容
        /// </summary>		
        public string Contents { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>		
        public string UserName { get; set; }
        /// <summary>
        /// 需求编号
        /// </summary>		
        public string DemandCode { get; set; }
        /// <summary>
        /// 代码文件名称
        /// </summary>		
        public string CodeFile { get; set; }

    }
}
