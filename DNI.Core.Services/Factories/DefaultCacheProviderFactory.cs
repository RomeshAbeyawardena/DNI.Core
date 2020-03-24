namespace DNI.Core.Services.Factories
{
    using System;
    using DNI.Core.Contracts;
    using DNI.Core.Contracts.Enumerations;
    using DNI.Core.Contracts.Factories;
    using DNI.Core.Contracts.Services;

    internal sealed class DefaultCacheProviderFactory : ICacheProviderFactory
    {
        private readonly IServiceProvider serviceProvider;
        private ISwitch<CacheType, Type> cacheServiceType;

        public DefaultCacheProviderFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            cacheServiceType = Switch.Create<CacheType, Type>()
                .CaseWhen(
                    CacheType.DistributedMemoryCache,
                    typeof(DefaultDistributedCacheService))
                .CaseWhen(
                    CacheType.SessionCache,
                    typeof(DefaultSessionCacheService));
        }

        public ICacheService GetCache(CacheType cacheType)
        {
            return (ICacheService)serviceProvider
                .GetService(cacheServiceType.Case(cacheType));
        }
    }
}
