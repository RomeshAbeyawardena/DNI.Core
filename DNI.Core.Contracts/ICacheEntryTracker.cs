namespace DNI.Core.Contracts
{
    using System.Threading;
    using System.Threading.Tasks;
    using DNI.Core.Contracts.Enumerations;

    public interface ICacheEntryTracker
    {
        Task SetState(string cacheItem, CacheEntryState cacheEntryState, CancellationToken cancellationToken);

        Task<CacheEntryState> GetState(string cacheItem, CancellationToken cancellationToken);
    }
}
