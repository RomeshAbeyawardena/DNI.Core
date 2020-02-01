using DNI.Shared.Contracts.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Options
{
    public static class DefaultPagerResult
    {
        public static IPagerResult<T> Create<T>(IQueryable<T> query)
        {
            return DefaultPagerResult<T>
                .Create(query);
        }
    }

    public class DefaultPagerResult<T> : IPagerResult<T>
    {
        private readonly IQueryable<T> _query;
        
        public int Length => _query.Count();
        public Task<int> LengthAsync => _query.CountAsync();
        
        public async Task<IEnumerable<T>> GetItems(int pageNumber, int maximumRowsPerPage, bool useAsync = true, CancellationToken cancellationToken = default)
        {
            var query = _query;

            var rowsToSkip = (pageNumber - 1) * maximumRowsPerPage;
            
            if(rowsToSkip > 0)
                query = query.Skip(rowsToSkip);

            var totalRows = useAsync 
                ? await query.CountAsync(cancellationToken) 
                : query.Count();

            if(totalRows > maximumRowsPerPage)
                query = query.Take(maximumRowsPerPage);

            return useAsync 
                ? await query.ToArrayAsync(cancellationToken) 
                : await Task.FromResult(query.ToArray());
        }

        public static IPagerResult<T> Create(IQueryable<T> query)
        {
            return new DefaultPagerResult<T>(query);
        }

        private DefaultPagerResult(IQueryable<T> query)
        {
            _query = query;
        }
    }
}
