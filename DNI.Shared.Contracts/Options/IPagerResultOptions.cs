using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Options
{
    public interface IPagerResultOptions
    {
        int PageNumber { get; set; }
        int MaximumRowsPerPage { get; set; }
        bool UseAsync { get; set; }
    }
}
