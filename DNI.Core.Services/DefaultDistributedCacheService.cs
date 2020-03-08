using DNI.Core.Contracts;
using DNI.Core.Services.Abstraction;
using DNI.Core.Services.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("DNI.Core.UnitTests")]
namespace DNI.Core.Services
{
    internal sealed class DefaultDistributedCacheService : DefaultCacheServiceBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDictionary<Type, IEnumerable<Type>> _entityCacheRules;

        private readonly IDistributedCache _distributedCache;
        private readonly DistributedCacheEntryOptions _distributedCacheEntryOptions;

        public override async Task<T> Get<T>(string cacheKeyName, CancellationToken cancellationToken)
        {
            var result = await _distributedCache.GetAsync(cacheKeyName, cancellationToken).ConfigureAwait(false);

            if(result == null || result.Length < 1)
                return default;

            return await Deserialise<T>(result).ConfigureAwait(false);
        }

        public override async Task Set<T>(string cacheKeyName, T value, CancellationToken cancellationToken = default)
        {
            if(value == null)
                return;

            var serialisedValue = await Serialise(value).ConfigureAwait(false);

            await _distributedCache.SetAsync(cacheKeyName, serialisedValue.ToArray(), 
                _distributedCacheEntryOptions, cancellationToken).ConfigureAwait(false);
        }

        public override async Task<T> Set<T>(string cacheKeyName, Func<T> getValue, CancellationToken cancellationToken = default)
        {
            var value = getValue();

            await Set(cacheKeyName, value, cancellationToken);

            return value;
        }

        public override async Task<T> Set<T>(string cacheKeyName, Func<CancellationToken, Task<T>> getValue, CancellationToken cancellationToken = default)
        {
            var value = await getValue(cancellationToken);

            await Set(cacheKeyName, value, cancellationToken);

            return value;
        }

        private IEnumerable<ICacheEntityRule<T>> GetCacheEntityRules<T>()
        {
            var cacheRulesList = new List<ICacheEntityRule<T>>();
            if(!_entityCacheRules.TryGetValue(typeof(T), out var cacheRuleTypes))
            foreach(var cacheRuleType in cacheRuleTypes)
            {
                cacheRulesList.Add(
                    _serviceProvider.CreateInjectedInstance(cacheRuleType));
            }

            return cacheRulesList;
        }

        public DefaultDistributedCacheService(IServiceProvider serviceProvider,  IDictionary<Type, IEnumerable<Type>> entityCacheRules, IDistributedCache distributedCache, 
            IMessagePackService messagePackService, IOptions<DistributedCacheEntryOptions> options)
            : base(messagePackService)
        {
            _serviceProvider = serviceProvider;
            _entityCacheRules = entityCacheRules;
            _distributedCache = distributedCache;
            _distributedCacheEntryOptions = options.Value;
        }
    }
}
