namespace DNI.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DNI.Core.Contracts;
    using DNI.Core.Contracts.Enumerations;
    using DNI.Core.Contracts.Stores;
    using Microsoft.Extensions.Logging;

    internal sealed class DefaultCacheEntryTracker : ICacheEntryTracker
    {
        private readonly ILogger<ICacheEntryTracker> log;
        private readonly ICacheTrackerStore cacheTrackerStore;

        public DefaultCacheEntryTracker(ILogger<ICacheEntryTracker> log, ICacheTrackerStore cacheTrackerStore)
        {
            this.log = log;
            this.cacheTrackerStore = cacheTrackerStore;
        }

        public async Task<CacheEntryState> GetState(string cacheItem, CancellationToken cancellationToken)
        {
            var items = await GetOrSetItems(true, cancellationToken);
            if (items.TryGetValue(cacheItem, out var cacheEntryState))
            {
                log.LogInformation("Retrieved state information for '{0}'", cacheItem);
                return cacheEntryState;
            }

            log.LogInformation("State information for '{0}' currently unavailable, state has been marked as new.", cacheItem);
            return CacheEntryState.New;
        }

        public async Task SetState(string cacheItem, CacheEntryState cacheEntryState, CancellationToken cancellationToken)
        {
            var items = await GetOrSetItems(false, cancellationToken);

            if (items.ContainsKey(cacheItem))
            {
                items[cacheItem] = cacheEntryState;
            }
            else
            {
                items.Add(cacheItem, cacheEntryState);
            }

            await cacheTrackerStore.SaveItems(items, cancellationToken);
            log.LogInformation("Writing state information for '{0}'", cacheItem);
            log.LogDebug("Setting {0} state for {1}", Enum.GetName(typeof(CacheEntryState), cacheEntryState), cacheItem);
        }

        private async Task<IDictionary<string, CacheEntryState>> GetOrSetItems(bool commitChanges, CancellationToken cancellationToken)
        {
            var items = await cacheTrackerStore.GetItems(cancellationToken);
            if (items == null)
            {
                log.LogInformation("State information currently unavailable, creating new...");
            }

            if (items == null && commitChanges)
            {
                await cacheTrackerStore.SaveItems(items = new Dictionary<string, CacheEntryState>(), cancellationToken);
            }

            return items ?? new Dictionary<string, CacheEntryState>();
        }
    }
}
