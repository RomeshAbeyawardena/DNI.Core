using DNI.Core.Contracts;
using DNI.Core.Contracts.Stores;
using DNI.Core.Contracts.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Services
{
    internal class DefaultCacheEntryTracker : ICacheEntryTracker
    {
        private readonly ICacheTrackerStore _cacheTrackerStore;

        public DefaultCacheEntryTracker(ICacheTrackerStore cacheTrackerStore)
        {
            _cacheTrackerStore = cacheTrackerStore;
        }

        public async Task<CacheEntryState> GetState(string cacheItem, CancellationToken cancellationToken)
        {
            var items = await GetOrSetItems(true, cancellationToken);
            if(items.TryGetValue(cacheItem, out var cacheEntryState))
                return cacheEntryState;
            
            return CacheEntryState.Invalid;
        }

        public async Task SetState(string cacheItem, CacheEntryState cacheEntryState, CancellationToken cancellationToken)
        {
            var items = await GetOrSetItems(false, cancellationToken);

            if(items.ContainsKey(cacheItem))
                items[cacheItem] = cacheEntryState;
            else
                items.Add(cacheItem, cacheEntryState);

            await _cacheTrackerStore.SaveItems(items, cancellationToken);
        }

        private async Task<IDictionary<string, CacheEntryState>> GetOrSetItems(bool commitChanges, CancellationToken cancellationToken)
        {
            var items = await _cacheTrackerStore.GetItems(cancellationToken);

            if(items == null && commitChanges)
                await _cacheTrackerStore.SaveItems(items = new Dictionary<string, CacheEntryState>(), cancellationToken);

            return items ?? new Dictionary<string, CacheEntryState>();
        }
    }
}
