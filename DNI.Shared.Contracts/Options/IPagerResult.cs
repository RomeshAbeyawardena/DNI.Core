using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Options
{
    public interface IPagerResult<T>
    { 
        int MaximumRowsPerPage { get; set; }
        Task<IEnumerable<T>> GetItems(int pageNumber, bool useAsync = true, CancellationToken cancellationToken = default);
    }
}
