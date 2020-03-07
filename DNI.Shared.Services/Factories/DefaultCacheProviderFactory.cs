using DNI.Core.Contracts;
using DNI.Core.Contracts.Enumerations;
using DNI.Core.Contracts.Factories;
using DNI.Core.Contracts.Services;
using System;

namespace DNI.Core.Services.Factories
{
    internal sealed class DefaultCacheProviderFactory : ICacheProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private ISwitch<CacheType, Type> _cacheServiceType;
        public DefaultCacheProviderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _cacheServiceType = Switch.Create<CacheType, Type>()
                .CaseWhen(CacheType.DistributedMemoryCache, 
                    typeof(DefaultDistributedCacheService))
                .CaseWhen(CacheType.SessionCache,
                    typeof(DefaultSessionCacheService));
        }

        public ICacheService GetCache(CacheType cacheType)
        {
            return (ICacheService)_serviceProvider
                .GetService(_cacheServiceType.Case(cacheType));
        }
    }
}
