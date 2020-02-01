using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Options;
using DNI.Shared.Contracts.Convertors;
using DNI.Shared.Contracts.Factories;
using DNI.Shared.Contracts.Managers;
using DNI.Shared.Contracts.Providers;
using DNI.Shared.Contracts.Services;
using DNI.Shared.Services.Convertors;
using DNI.Shared.Services.Factories;
using DNI.Shared.Services.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Microsoft.IO;
using DNI.Shared.Services.Options;

namespace DNI.Shared.Services
{
    public class ServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services, IServiceRegistrationOptions options)
        {
            services
                .AddSingleton<ISystemClock, SystemClock>()
                .AddSingleton<IClockProvider, DefaultClockProvider>()
                .AddSingleton(new RecyclableMemoryStreamManager())
                .AddSingleton<IHashingProvider, HashingProvider>()
                .AddSingleton<IClaimTypeValueConvertor, DefaultClaimTypeValueConvertor>()
                .AddSingleton<IModifierFlagPropertyService, ModifierFlagPropertyService>()
                .AddSingleton<IDefaultValueSetterService, DefaultValueSetterService>()
                .AddSingleton<IJsonWebTokenService, JsonWebTokenService>()
                .AddSingleton<IMemoryStreamManager, MemoryStreamManager>()
                .AddSingleton<ICryptographyProvider, CryptographyProvider>();

            if (options.RegisterMessagePackSerialisers)
                services
                    .AddSingleton<IMessagePackService, MessagePackService>();

            if(options.RegisterAutoMappingProviders)
                services
                    .AddSingleton<IMapperProvider, MapperProvider>();

            if (options.RegisterCacheProviders)
                services
                    .AddScoped(serviceProvider => serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext.Session)
                    .AddScoped<DefaultDistributedCacheService>()
                    .AddScoped<DefaultSessionCacheService>()
                    .AddScoped<ICacheProviderFactory, DefaultCacheProviderFactory>()
                    .AddScoped<ICacheProvider, DefaultCacheProvider>()
                    .AddTransient<IMediatorService, MediatorService>();
        }
    }
}
