using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression = default);
        Task<TEntity> Find(params object[] keys);
        Task<TEntity> SaveChanges(TEntity entity, bool saveChanges = true);
    }
}
