using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Enumerations;
using DNI.Shared.Contracts.Factories;
using DNI.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Factories
{
    public class DefaultCacheProviderFactory : ICacheProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;
        protected ISwitch<CacheType, Type> _cacheServiceType;
        public DefaultCacheProviderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _cacheServiceType
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
