﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts
{
    public interface IAsyncQueryResultTransformer<T>
        where T: class
    {
        Task<IEnumerable<T>> ToArrayAsync(CancellationToken cancellationToken);
        Task<TSelector> ToMaxAsync<TSelector>(Expression<Func<T, TSelector>> selectorExpression, CancellationToken cancellationToken);
        Task<decimal?> ToSumAsync(Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken);
        Task<int> ToCountAsync(CancellationToken cancellationToken);
        Task<long> ToLongCountAsync(CancellationToken cancellationToken);
        Task<IList<T>> ToListAsync(CancellationToken cancellationToken);
        Task<T> ToFirstAsync(CancellationToken cancellationToken);
        Task<T> ToFirstOrDefaultAsync(CancellationToken cancellationToken);
        Task<T> ToSingleAsync(CancellationToken cancellationToken);
        Task<T> ToSingleOrDefaultAsync(CancellationToken cancellationToken);
        IQueryable<T> AsNoTracking(IQueryable<T> query);
        IPagerResult<T> AsPager(IQueryable<T> query);
    }
}
