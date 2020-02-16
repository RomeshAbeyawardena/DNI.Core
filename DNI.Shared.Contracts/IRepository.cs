using DNI.Shared.Contracts.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression = default, bool enableTracking = true);
        Task<TEntity> Find(CancellationToken cancellationToken = default, bool enableTracking = true, params object[] keys);
        Task<TEntity> SaveChanges(TEntity entity, bool saveChanges = true, 
            bool detachAfterSave = true,  CancellationToken cancellationToken = default);
        IPagerResult<TEntity> GetPager(IQueryable<TEntity> query);
        IQueryable<TEntity> AsNoTracking(IQueryable<TEntity> query);
    }
}
