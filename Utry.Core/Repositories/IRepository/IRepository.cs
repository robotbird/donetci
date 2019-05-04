using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utry.Core.Repositories.IRepository
{
    /// <summary>
    /// Repository
    /// </summary>
    public partial interface IRepository<T>
    {
        T GetById(object id);
        int Insert(T entity);
        int Update(T entity);
        int Delete(T entity);
        IEnumerable<T> Table { get; }
    }
}
