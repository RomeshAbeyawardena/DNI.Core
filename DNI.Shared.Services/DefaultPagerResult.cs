using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Options;
using DNI.Shared.Services.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    public static class DefaultPagerResult
    {
        public static IPagerResult<T> Create<T>(IQueryable<T> query)
        {
            return DefaultPagerResult<T>
                .Create(query);
        }
    }

    internal sealed class DefaultPagerResult<T> : IPagerResult<T>
    {
        private readonly IQueryable<T> _query;
        
        public int Length => _query.Count();
        public Task<int> LengthAsync => _query.CountAsync();
        public async Task<int> GetTotalNumberOfPages(int maximumRowsPerPage, bool useAsync = true)
        {
            var length = useAsync 
                ? await LengthAsync 
                : await Task.FromResult(Length);

            if(maximumRowsPerPage == 0 || length == 0)
                return 0;

            var totalPageNumbersinDecimal = Convert.ToDecimal(length) / Convert.ToDecimal(maximumRowsPerPage);

            return Convert.ToInt32(
                Math.Ceiling(totalPageNumbersinDecimal));
        }

        public async Task<IEnumerable<T>> GetItems(int pageNumber, int maximumRowsPerPage, 
            bool useAsync = true, CancellationToken cancellationToken = default)
        {
            return await GetPagedItems(pagerResultOptions => {
                pagerResultOptions.PageNumber = pageNumber; 
                pagerResultOptions.MaximumRowsPerPage = 
                maximumRowsPerPage; pagerResultOptions.UseAsync = useAsync; 
            }, cancellationToken);
        }

        public static IPagerResult<T> Create(IQueryable<T> query)
        {
            return new DefaultPagerResult<T>(query);
        }

        public async Task<IEnumerable<T>> GetPagedItems(Action<IPagerResultOptions> pagerResultOptionsBuilder, CancellationToken cancellationToken = default)
        {
            var pagerResultOptions = new PagerResultOptions();

            pagerResultOptionsBuilder(pagerResultOptions);

            var query = _query;

            var rowsToSkip = (pagerResultOptions.PageNumber - 1) * pagerResultOptions.MaximumRowsPerPage;

            if (rowsToSkip > 0)
                query = query.Skip(rowsToSkip);

            var totalRows = pagerResultOptions.UseAsync
                ? await query.CountAsync(cancellationToken)
                : query.Count();

            if (totalRows > pagerResultOptions.MaximumRowsPerPage)
                query = query.Take(pagerResultOptions.MaximumRowsPerPage);

            return pagerResultOptions.UseAsync
                ? await query.ToArrayAsync(cancellationToken)
                : await Task.FromResult(query.ToArray());
        }

        private DefaultPagerResult(IQueryable<T> query)
        {
            _query = query;
        }
    }
}
