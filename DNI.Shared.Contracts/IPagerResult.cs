using DNI.Shared.Contracts.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts
{
    public interface IPagerResult<T>
    { 
        int Length { get; }
        Task<int> LengthAsync { get; }
        Task<int> GetTotalNumberOfPages(int maximumRowsPerPage, bool useAsync = true);
        [Obsolete("Use GetPagedItems instead.")]
        Task<IEnumerable<T>> GetItems(int pageNumber, int maximumRowsPerPage, 
            bool useAsync = true, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetPagedItems(Action<IPagerResultOptions> pagerResultOptions, CancellationToken cancellationToken = default);
    }
}
