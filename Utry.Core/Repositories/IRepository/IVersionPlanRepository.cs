using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utry.Core.Domain;


namespace Utry.Core.Repositories.IRepository
{
    public partial interface IVersionPlanRepository:IRepository<CIVersionPlan>
    {
        List<CIVersionPlan> GetPlanList(int pageSize, int pageIndex, out int recordCount, string where);
    }
}
