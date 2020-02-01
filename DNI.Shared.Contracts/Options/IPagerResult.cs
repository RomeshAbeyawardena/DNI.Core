using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Options
{
    public interface IPagerResult<T>
    {
        int PageNumber { get; }
        int MaximumRowsPerPage { get; }
        Task<IEnumerable<T>> GetItems();
    }
}
