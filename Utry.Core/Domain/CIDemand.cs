using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utry.Core.Domain
{
    public class CIDemand
    {
        /// <summary>
        /// 需求编号
        /// </summary>
        public string DemandNumber { get; set; }

        public string DemandState { get; set; }

        public string VersionNum { get; set; }
    }
}
