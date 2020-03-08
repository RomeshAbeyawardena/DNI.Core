using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IAsyncQueryResultTransformer<T> : IQueryResultTransformer<T>
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
        Task<bool> AnyAsync(CancellationToken cancellation);
    }
}
