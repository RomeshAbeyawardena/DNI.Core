using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Convertors;
using DNI.Shared.Contracts.Factories;
using DNI.Shared.Contracts.Managers;
using DNI.Shared.Contracts.Providers;
using DNI.Shared.Contracts.Services;
using DNI.Shared.Services.Convertors;
using DNI.Shared.Services.Factories;
using DNI.Shared.Services.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Microsoft.IO;

namespace DNI.Shared.Services
{
    public class ServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<ISystemClock, SystemClock>()
                .AddSingleton<IClockProvider, DefaultClockProvider>()
                .AddSingleton(new RecyclableMemoryStreamManager())
                .AddSingleton<IMessagePackService, MessagePackService>()
                .AddSingleton<IHashingProvider, HashingProvider>()
                .AddSingleton<IMapperProvider, MapperProvider>()
                .AddSingleton<IClaimTypeValueConvertor, DefaultClaimTypeValueConvertor>()
                .AddSingleton<IModifierFlagPropertyService, ModifierFlagPropertyService>()
                .AddSingleton<IDefaultValueSetterService, DefaultValueSetterService>()
                .AddSingleton<IJsonWebTokenService, JsonWebTokenService>()
                .AddSingleton<IMemoryStreamManager, MemoryStreamManager>()
                .AddSingleton<ICryptographyProvider, CryptographyProvider>()
                .AddSingleton<ICacheProviderFactory, DefaultCacheProviderFactory>()
                .AddTransient<IMediatorService, MediatorService>();
        }
    }
}
