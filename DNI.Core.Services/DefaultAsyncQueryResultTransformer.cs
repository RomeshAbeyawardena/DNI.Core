namespace DNI.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using DNI.Core.Contracts;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query;

    internal class DefaultAsyncQueryResultTransformer
    {
        public static IAsyncQueryResultTransformer<T> Create<T>(IQueryable<T> query)
            where T : class
        {
            return DefaultAsyncQueryResultTransformer<T>.Create(query);
        }
    }

    internal class DefaultAsyncQueryResultTransformer<T> : IAsyncQueryResultTransformer<T>
        where T : class
    {
        private readonly IQueryable<T> query;

        public static DefaultAsyncQueryResultTransformer<T> Create(IQueryable<T> query)
        {
            return new DefaultAsyncQueryResultTransformer<T>(query);
        }

        private DefaultAsyncQueryResultTransformer(IQueryable<T> query)
        {
            this.query = query;
        }

        public async Task<IEnumerable<T>> ToArrayAsync(CancellationToken cancellationToken)
        {
            return await query.ToArrayAsync(cancellationToken);
        }

        public async Task<T> ToFirstAsync(CancellationToken cancellationToken)
        {
            return await query.FirstAsync(cancellationToken);
        }

        public async Task<T> ToFirstOrDefaultAsync(CancellationToken cancellationToken)
        {
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IList<T>> ToListAsync(CancellationToken cancellationToken)
        {
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<T> ToSingleAsync(CancellationToken cancellationToken)
        {
            return await query.SingleAsync(cancellationToken);
        }

        public async Task<T> ToSingleOrDefaultAsync(CancellationToken cancellationToken)
        {
            return await query.SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<decimal?> ToSumAsync(Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken)
        {
            return await query.SumAsync(selector, cancellationToken);
        }

        public async Task<int> ToCountAsync(CancellationToken cancellationToken)
        {
            return await query.CountAsync(cancellationToken);
        }

        public async Task<long> ToLongCountAsync(CancellationToken cancellationToken)
        {
            return await query.LongCountAsync(cancellationToken);
        }

        public async Task<TSelector> ToMaxAsync<TSelector>(Expression<Func<T, TSelector>> selectorExpression, CancellationToken cancellationToken)
        {
            return await query.MaxAsync(selectorExpression, cancellationToken);
        }

        public IQueryable<T> AsNoTracking()
        {
            return query.AsNoTracking();
        }

        public IPagerResult<T> AsPager()
        {
            return DefaultPagerResult
                .Create(query);
        }

        public async Task<bool> AnyAsync(CancellationToken cancellation)
        {
            return await query.AnyAsync(cancellation);
        }

        public IIncludableQueryable<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath = null, string propertyName = null)
        {
            return query.Include(navigationPropertyPath);
        }
    }
}
