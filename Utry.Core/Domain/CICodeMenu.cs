using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utry.Core.Domain
{
    public class CICodeMenu
    {

        /// <summary>
        /// 主键guid
        /// </summary>		
        public string ID { get; set; }
        /// <summary>
        /// 代码版本
        /// </summary>		
        public string CodeVersion { get; set; }
        /// <summary>
        /// 文件相对路径
        /// </summary>		
        public string CodePath { get; set; }
        /// <summary>
        /// 所属工程名称
        /// </summary>		
        public string PrjName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>		
        public string Status { get; set; }

    }
}
