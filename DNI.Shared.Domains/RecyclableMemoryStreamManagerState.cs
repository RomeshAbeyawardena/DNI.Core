using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Domains
{
    public class RecyclableMemoryStreamManagerState
    {
        public bool BlockDiscarded { get; set; }
        public long SmallPoolInUseBytes { get; set; }
        public long SmallPoolFreeBytes { get; set; }
        public long LargePoolInUseBytes { get; set; }
        public long LargePoolFreeBytes { get; set; }
        public bool BlockCreated { get; set; }
        public bool StreamCreated { get; set; }
        public bool StreamDisposed { get; set; }
        public bool StreamFinalized { get; set; }
        public bool UsageReportRequested { get; set; }
    }
}
