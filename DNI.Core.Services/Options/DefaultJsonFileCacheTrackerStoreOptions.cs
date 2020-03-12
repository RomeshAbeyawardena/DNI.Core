using DNI.Core.Contracts.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Options
{
    internal class DefaultJsonFileCacheTrackerStoreOptions : IJsonFileCacheTrackerStoreOptions
    {
        public string FileName { get; set; }
    }
}
