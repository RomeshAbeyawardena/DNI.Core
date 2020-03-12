using DNI.Core.Contracts;
using DNI.Core.Contracts.Stores;
using DNI.Core.Contracts.Enumerations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace DNI.Core.Services
{
    internal sealed class DefaultCacheEntryTracker : ICacheEntryTracker
    {
        private readonly ILogger<ICacheEntryTracker> _log;
        private readonly ICacheTrackerStore _cacheTrackerStore;

        public DefaultCacheEntryTracker(ILogger<ICacheEntryTracker> log, ICacheTrackerStore cacheTrackerStore)
        {
            _log = log;
            _cacheTrackerStore = cacheTrackerStore;
        }

        public async Task<CacheEntryState> GetState(string cacheItem, CancellationToken cancellationToken)
        {
            var items = await GetOrSetItems(true, cancellationToken);
            if(items.TryGetValue(cacheItem, out var cacheEntryState))
            {
                _log.LogInformation("Retrieved state information for '{0}'", cacheItem);
                return cacheEntryState;
            }

            _log.LogInformation("State information for '{0}' currently unavailable, state has been marked as new.", cacheItem);
            return CacheEntryState.New;
        }

        public async Task SetState(string cacheItem, CacheEntryState cacheEntryState, CancellationToken cancellationToken)
        {
            var items = await GetOrSetItems(false, cancellationToken);

            if(items.ContainsKey(cacheItem))
                items[cacheItem] = cacheEntryState;
            else
                items.Add(cacheItem, cacheEntryState);

            await _cacheTrackerStore.SaveItems(items, cancellationToken);
            _log.LogInformation("Writing state information for '{0}'", cacheItem);
            _log.LogDebug("Setting {0} state for {1}", Enum.GetName(typeof(CacheEntryState), cacheEntryState), cacheItem);
        }

        private async Task<IDictionary<string, CacheEntryState>> GetOrSetItems(bool commitChanges, CancellationToken cancellationToken)
        {
            var items = await _cacheTrackerStore.GetItems(cancellationToken);
            if(items == null)
                _log.LogInformation("State information currently unavailable, creating new...");

            if(items == null && commitChanges)
                await _cacheTrackerStore.SaveItems(items = new Dictionary<string, CacheEntryState>(), cancellationToken);
            
            return items ?? new Dictionary<string, CacheEntryState>();
        }
    }
}
