using DNI.Shared.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    internal class DefaultAsyncResultTransformer
    {
        public static IAsyncResultTransformer<T> Create<T>(IQueryable<T> query)
        {
            return DefaultAsyncResultTransformer<T>.Create(query);
        }
    }

    internal class DefaultAsyncResultTransformer<T> : IAsyncResultTransformer<T>
    {
        private readonly IQueryable<T> _query;

        public static DefaultAsyncResultTransformer<T> Create(IQueryable<T> query)
        {
            return new DefaultAsyncResultTransformer<T>(query);
        }

        private DefaultAsyncResultTransformer(IQueryable<T> query)
        {
            _query = query;
        }

        public async Task<IEnumerable<T>> ArrayAsync(CancellationToken cancellationToken)
        {
            return await _query.ToArrayAsync(cancellationToken);
        }

        public async Task<T> FirstAsync(CancellationToken cancellationToken)
        {
            return await _query.FirstAsync(cancellationToken);
        }

        public async Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken)
        {
            return await _query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IList<T>> ListAsync(CancellationToken cancellationToken)
        {
            return await _query.ToListAsync(cancellationToken);
        }

        public async Task<T> SingleAsync(CancellationToken cancellationToken)
        {
            return await _query.SingleAsync(cancellationToken);
        }

        public async Task<T> SingleOrDefaultAsync(CancellationToken cancellationToken)
        {
            return await _query.SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<decimal?> SumAsync(Expression<Func<T,decimal?>> selector, CancellationToken cancellationToken)
        {
            return await _query.SumAsync(selector, cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return await _query.CountAsync(cancellationToken);
        }

        public async Task<long> LongCountAsync(CancellationToken cancellationToken)
        {
            return await _query.LongCountAsync(cancellationToken);
        }
    }
}
