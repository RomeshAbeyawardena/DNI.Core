﻿using DNI.Shared.Contracts.Options;

namespace DNI.Shared.Services.Options
{
    internal sealed class DefaultPagerResultOptions : IPagerResultOptions
    {
        public int PageNumber { get; set; }
        public int MaximumRowsPerPage { get; set; }
        public bool UseAsync { get; set; }
}
}
