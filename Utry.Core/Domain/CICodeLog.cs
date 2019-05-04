using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utry.Core.Domain
{
    public class CICodeLog
    {

        /// <summary>
        /// 主键guid
        /// </summary>		
        public string ID { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>		
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 代码内容
        /// </summary>		
        public string CodeContent { get; set; }
        /// <summary>
        /// 需求或者bug编号
        /// </summary>		
        public string DemandCode { get; set; }
        /// <summary>
        /// checkitem表主键
        /// </summary>		
        public string CheckListId { get; set; }

    }
}
