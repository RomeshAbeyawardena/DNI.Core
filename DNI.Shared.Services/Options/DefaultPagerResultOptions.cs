using DNI.Shared.Contracts.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Options
{
    internal sealed class DefaultPagerResultOptions : IPagerResultOptions
    {
        public int PageNumber { get; set; }
        public int MaximumRowsPerPage { get; set; }
        public bool UseAsync { get; set; }
}
}
