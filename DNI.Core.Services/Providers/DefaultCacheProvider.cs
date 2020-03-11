using DNI.Core.Contracts;
using DNI.Core.Contracts.Enumerations;
using DNI.Core.Contracts.Factories;
using DNI.Core.Contracts.Providers;
using DNI.Core.Contracts.Services;
using DNI.Core.Services.Abstraction;
using DNI.Core.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("DNI.Core.UnitTests")]
namespace DNI.Core.Services.Providers
{

    internal sealed class DefaultCacheProvider : ICacheProvider
    {
        private readonly ICacheProviderFactory _cacheProviderFactory;

        private readonly IServiceProvider _serviceProvider;
        private readonly IDictionary<Type, IEnumerable<Type>> _entityCacheRules;

        public DefaultCacheProvider(ICacheProviderFactory cacheProviderFactory,
            IServiceProvider serviceProvider, IDictionary<Type, IEnumerable<Type>> entityCacheRules)
        {
            _cacheProviderFactory = cacheProviderFactory;
            _serviceProvider = serviceProvider;
            _entityCacheRules = entityCacheRules;
        }

        public async Task<T> Get<T>(CacheType cacheType, string cacheKeyName, CancellationToken cancellationToken = default)
        {
            var cacheService = GetCacheService(cacheType);

            return await cacheService.Get<T>(cacheKeyName, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetOrSet<T>(CacheType cacheType, string cacheKeyName,
            Func<CancellationToken, Task<IEnumerable<T>>> getValue, bool append = false,
            CancellationToken cancellationToken = default)
        {
            var value = await Get<IEnumerable<T>>(cacheType, cacheKeyName, cancellationToken);

            async Task<IEnumerable<T>> getDataFromSource()
            {
                value = await Set(cacheType, cacheKeyName, async (cancellationToken) =>
                    await getValue(cancellationToken));

                return value;
            };

            if (value == null || !value.Any())
                return await getDataFromSource();

            await ValidateCacheEntityRules(getDataFromSource, value);

            return value;
        }

        public async Task ValidateCacheEntityRules<T>(RequiresRefreshDelegate requiresRefresh, IEnumerable<T> currentValue)
        {
            
            if (!_entityCacheRules.TryGetValue(typeof(T), out var ruleTypes))
                return;

            var ruleTypesArray = ruleTypes.ToArray();
            var currentIndex = 0;

            ICacheEntityRule<T> GetNext(int index, CacheEntityRuleDelegate<T> cacheEntityRule)
            {
                if(index < ruleTypesArray.Length)
                    return (ICacheEntityRule<T>)Activator.CreateInstance(ruleTypesArray[index], requiresRefresh, cacheEntityRule);

                return default;
            }

            CacheEntityRuleDelegate<T> cacheEntityRule = null;
            ICacheEntityRule<T> next;
            cacheEntityRule = async (serviceProvider, values) =>
            {
                next = GetNext(currentIndex++, cacheEntityRule);

                if(next == null || !await next.IsEnabled(serviceProvider))
                    return;

                await next.OnGet(serviceProvider, values);
            };

            next = GetNext(currentIndex++, cacheEntityRule);
            await next?.OnGet(_serviceProvider, currentValue);

        }

        public async Task<T> GetOrSet<T>(CacheType cacheType, string cacheKeyName,
            Func<CancellationToken, Task<T>> getValue, bool append = false,
            CancellationToken cancellationToken = default)
        {
            var value = await Get<T>(cacheType, cacheKeyName, cancellationToken);

            return await Set(cacheType, cacheKeyName,
                    async (cancellationToken) => await getValue(cancellationToken), cancellationToken);
        }

        public async Task Set<T>(CacheType cacheType, string cacheKeyName, T value, CancellationToken cancellationToken = default)
        {
            if (value == null)
                return;

            var cacheService = GetCacheService(cacheType);
            await cacheService.Set(cacheKeyName, value, cancellationToken);

        }

        public async Task<T> Set<T>(CacheType cacheType, string cacheKeyName,
            Func<T> getValue, CancellationToken cancellationToken = default)
        {
            var value = getValue();

            await Set(cacheType, cacheKeyName, value, cancellationToken);
            return value;
        }

        public async Task<T> Set<T>(CacheType cacheType, string cacheKeyName,
            Func<CancellationToken, Task<T>> getValue,
            CancellationToken cancellationToken = default)
        {
            var value = await getValue(cancellationToken);

            await Set(cacheType, cacheKeyName, value, cancellationToken);

            return value;
        }

        private ICacheService GetCacheService(CacheType cacheType)
        {
            return _cacheProviderFactory.GetCache(cacheType);
        }

    }
}
