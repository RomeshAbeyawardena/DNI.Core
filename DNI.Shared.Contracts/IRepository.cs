﻿using DNI.Shared.Contracts.Options;
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
        Action<TEntity> ConfigureSoftDeletion { get; set; }
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression = default, bool enableTracking = true);
        Task<TEntity> Find(bool enableTracking = true, CancellationToken cancellationToken = default, params object[] keys);
        Task<TEntity> SaveChanges(TEntity entity, bool saveChanges = true, 
            bool detachAfterSave = true,  CancellationToken cancellationToken = default);
        IPagerResult<TEntity> GetPager(IQueryable<TEntity> query);
        IQueryable<TEntity> AsNoTracking(IQueryable<TEntity> query);
        Task<int> Delete(TEntity entity, CancellationToken cancellationToken = default);
        Task<int> Delete(TEntity entity, bool softDelete = true, CancellationToken cancellationToken = default);
        Task<int> Delete(CancellationToken cancellationToken = default, params object[] keys);
        Task<int> Delete(bool softDelete = true, CancellationToken cancellationToken = default, params object[] keys);

    }
}
