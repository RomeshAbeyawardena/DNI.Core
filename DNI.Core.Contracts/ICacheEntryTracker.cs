using DNI.Core.Contracts.Enumerations;
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
