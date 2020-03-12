using DNI.Core.Contracts.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface ICacheEntryTracker
    {
        Task SetState(string cacheItem, CacheEntryState cacheEntryState, CancellationToken cancellationToken);
        Task<CacheEntryState> GetState(string cacheItem, CancellationToken cancellationToken);
    }
}
