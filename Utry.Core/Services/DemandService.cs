using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Utry.Core.Repositories.Repository;
using Utry.Core.Repositories.IRepository;
using Utry.Core.Domain;
using Utry.Framework.Web;
using Utry.Framework.Configuration;
using Utry.Framework.Utils;
using Utry.Framework.Mvc;

namespace Utry.Core.Services
{
    public class DemandService
    {

        private IDemandRepository _demandrepository;

        #region "构造函数"
        /// <summary>
        /// 构造函数
        /// </summary>
        public DemandService()
            : this(new DemandRepository())
        { 
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="reviewrepository"></param>
        public DemandService(IDemandRepository demandrepository)
        {
            this._demandrepository = demandrepository;
        }
        #endregion

        /// <summary>
        /// 获取全部需求编号
        /// </summary>
        /// <returns></returns>
        public List<CIDemand> GetDemandList()
        {
            return _demandrepository.Table.ToList();
        }
    }
}
