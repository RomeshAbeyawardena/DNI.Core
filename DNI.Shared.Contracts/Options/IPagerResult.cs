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
        Task<IEnumerable<T>> GetItems(int pageNumber, int maximumRowsPerPage, 
            bool useAsync = true, CancellationToken cancellationToken = default);
    }
}
