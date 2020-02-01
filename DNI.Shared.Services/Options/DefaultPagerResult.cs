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

            var totalRows = useAsync ? await query.CountAsync(cancellationToken) : query.Count();

            var rowsToSkip = Convert.ToDecimal(totalRows) / Convert.ToDecimal(pageNumber);

            if(rowsToSkip > MaximumRowsPerPage)
                query = query.Skip(Convert.ToInt32(Math.Truncate(rowsToSkip)));

            if((useAsync ? await query.CountAsync(cancellationToken) : query.Count()) > MaximumRowsPerPage)
                query = query.Take(MaximumRowsPerPage);

            return useAsync ? await query.ToArrayAsync(cancellationToken) : await Task.FromResult(query.ToArray());
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
