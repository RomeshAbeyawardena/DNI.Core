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

        public int MaximumRowsPerPage { get; set; }

        public async Task<IEnumerable<T>> GetItems(int pageNumber, bool useAsync = true, CancellationToken cancellationToken = default)
        {
            var query = _query;

            var rowsToSkip = (pageNumber - 1) * MaximumRowsPerPage;
            
            if(rowsToSkip > 0)
                query = query.Skip(rowsToSkip);

            var totalRows = useAsync 
                ? await query.CountAsync(cancellationToken) 
                : query.Count();

            if(totalRows > MaximumRowsPerPage)
                query = query.Take(MaximumRowsPerPage);

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
