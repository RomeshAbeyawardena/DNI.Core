using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.IO.RecyclableMemoryStreamManager.Events;

namespace DNI.Shared.Domains.States
{
    public class RecyclableMemoryStreamManagerState
    {
        public static void Handle(RecyclableMemoryStreamManagerState state,
            Action onStreamCreated = null,
            Action onBlockCreated = null,
            Action onBlockDiscarded = null,
            Action onStreamDisposed = null,
            Action onStreamFinalized = null,
            Action onLargeBufferCreated = null,
            Action<MemoryStreamDiscardReason> onLargeBufferDiscarded = null,
            Action<long, long, long, long> onUsageReportRequested = null)
        {
            if(state.StreamCreated)
                onStreamCreated?.Invoke();

            if(state.BlockCreated)
                onBlockCreated?.Invoke();

            if(state.BlockDiscarded)
                onBlockDiscarded?.Invoke();

            if(state.StreamDisposed)
                onStreamDisposed?.Invoke();

            if(state.StreamFinalized)
                onStreamFinalized?.Invoke();

            if(state.LargeBufferCreated)
                onLargeBufferCreated?.Invoke();

            if(state.LargeBufferDiscarded)
                onLargeBufferDiscarded?.Invoke(state.Reason);

            if(state.UsageReportRequested)
                onUsageReportRequested?.Invoke(state.LargePoolFreeBytes, 
                    state.LargePoolInUseBytes, state.SmallPoolFreeBytes,
                    state.SmallPoolInUseBytes);
        }
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
        public bool LargeBufferCreated { get; set; }
        public bool LargeBufferDiscarded { get; set; }
        public MemoryStreamDiscardReason Reason { get; set; }
    }
}
