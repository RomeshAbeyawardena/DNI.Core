using DNI.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Services
{
    internal class DefaultAsyncQueryResultTransformer
    {
        public static IAsyncQueryResultTransformer<T> Create<T>(IQueryable<T> query)
            where T: class
        {
            return DefaultAsyncQueryResultTransformer<T>.Create(query);
        }
    }

    internal class DefaultAsyncQueryResultTransformer<T>: IAsyncQueryResultTransformer<T>
        where T : class
    {
        private readonly IQueryable<T> _query;

        public static DefaultAsyncQueryResultTransformer<T> Create(IQueryable<T> query)
        {
            return new DefaultAsyncQueryResultTransformer<T>(query);
        }

        private DefaultAsyncQueryResultTransformer(IQueryable<T> query)
        {
            _query = query;
        }

        public async Task<IEnumerable<T>> ToArrayAsync(CancellationToken cancellationToken)
        {
            return await _query.ToArrayAsync(cancellationToken);
        }

        public async Task<T> ToFirstAsync(CancellationToken cancellationToken)
        {
            return await _query.FirstAsync(cancellationToken);
        }

        public async Task<T> ToFirstOrDefaultAsync(CancellationToken cancellationToken)
        {
            return await _query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IList<T>> ToListAsync(CancellationToken cancellationToken)
        {
            return await _query.ToListAsync(cancellationToken);
        }

        public async Task<T> ToSingleAsync(CancellationToken cancellationToken)
        {
            return await _query.SingleAsync(cancellationToken);
        }

        public async Task<T> ToSingleOrDefaultAsync(CancellationToken cancellationToken)
        {
            return await _query.SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<decimal?> ToSumAsync(Expression<Func<T,decimal?>> selector, CancellationToken cancellationToken)
        {
            return await _query.SumAsync(selector, cancellationToken);
        }

        public async Task<int> ToCountAsync(CancellationToken cancellationToken)
        {
            return await _query.CountAsync(cancellationToken);
        }

        public async Task<long> ToLongCountAsync(CancellationToken cancellationToken)
        {
            return await _query.LongCountAsync(cancellationToken);
        }

        public async Task<TSelector> ToMaxAsync<TSelector>(Expression<Func<T, TSelector>> selectorExpression, CancellationToken cancellationToken)
        {
            return await _query.MaxAsync(selectorExpression, cancellationToken);
        }

        public IQueryable<T> AsNoTracking(IQueryable<T> query)
        {
            return query.AsNoTracking();
        }

        public IPagerResult<T> AsPager(IQueryable<T> query)
        {
            return DefaultPagerResult
                .Create(query);
        }
    }
}
