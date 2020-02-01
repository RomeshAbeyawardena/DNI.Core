using DNI.Shared.Contracts.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Options
{
    public class DefaultPagerResult<T> : IPagerResult<T>
    {
        private readonly IQueryable<T> _query;

        public int PageNumber { get; }

        public int MaximumRowsPerPage { get; }

        public async Task<IEnumerable<T>> GetItems()
        {
            var query = _query;

            var totalRows = await query.CountAsync();

            var rowsToSkip = Convert.ToDecimal(totalRows) / Convert.ToDecimal(PageNumber);

            if(rowsToSkip > MaximumRowsPerPage)
                query = query.Skip(Convert.ToInt32(Math.Truncate(rowsToSkip)));

            if(await query.CountAsync() > MaximumRowsPerPage)
                query = query.Take(MaximumRowsPerPage);

            return await query.ToArrayAsync();
        }

        public DefaultPagerResult(IQueryable<T> query)
        {
            _query = query;
        }
    }
}
