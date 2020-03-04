using DNI.Shared.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<IEnumerable<T>> ArrayAsync()
        {
            return await _query.ToArrayAsync();
        }

        public async Task<T> FirstAsync()
        {
            return await _query.FirstAsync();
        }

        public async Task<T> FirstOrDefaultAsync()
        {
            return await _query.FirstOrDefaultAsync();
        }

        public async Task<IList<T>> ListAsync()
        {
            return await _query.ToListAsync();
        }

        public async Task<T> SingleAsync()
        {
            return await _query.SingleAsync();
        }

        public async Task<T> SingleOrDefaultAsync()
        {
            return await _query.SingleOrDefaultAsync();
        }
    }
}
