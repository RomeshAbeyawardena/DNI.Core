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
using MediatR;
using DNI.Shared.Services.Generators;
using DNI.Shared.Contracts.Generators;
using System.Security.Cryptography;

namespace DNI.Shared.Services
{
    public class ServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services, IServiceRegistrationOptions options)
        {
            services
                .AddSingleton(RandomNumberGenerator.Create())
                .AddSingleton<IRandomStringGenerator, DefaultRandomStringGenerator>()
                .AddSingleton<IHttpClientFactory, DefaultHttpClientFactory>()
                .AddSingleton<IGuidService, DefaultGuidService>()
                .AddSingleton<IMarkdownToHtmlService, DefaultMarkdownToHtmlService>()
                .AddSingleton<ISystemClock, SystemClock>()
                .AddSingleton<IClockProvider, DefaultClockProvider>()
                .AddSingleton(new RecyclableMemoryStreamManager())
                .AddSingleton<IHashingProvider, HashingProvider>()
                .AddSingleton<IClaimTypeValueConvertor, DefaultClaimTypeValueConvertor>()
                .AddSingleton<IModifierFlagPropertyService, DefaultModifierFlagPropertyService>()
                .AddSingleton<IDefaultValueSetterService, DefaultValueSetterService>()
                .AddSingleton<IJsonWebTokenService, DefaultJsonWebTokenService>()
                .AddSingleton<IMemoryStreamManager, DefaultMemoryStreamManager>()
                .AddSingleton<ICryptographyProvider, CryptographyProvider>()
                .AddSingleton<IEncryptionProvider,EncryptionProvider>();

            if(options.RegisterMediatorServices)
                services
                    .AddTransient(typeof(IPipelineBehavior<,>), typeof(DefaultValidationBehaviour<,>))
                    .AddTransient<IMediatorService, DefaultMediatorService>();

            if (options.RegisterMessagePackSerialisers)
                services
                    .AddSingleton<IMessagePackService, DefaultMessagePackService>();

            if(options.RegisterAutoMappingProviders)
                services
                    .AddSingleton<IMapperProvider, MapperProvider>();

            if (options.RegisterCacheProviders)
                services
                    .AddScoped(serviceProvider => serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext.Session)
                    .AddScoped<DefaultDistributedCacheService>()
                    .AddScoped<DefaultSessionCacheService>()
                    .AddScoped<ICacheProviderFactory, DefaultCacheProviderFactory>()
                    .AddScoped<ICacheProvider, DefaultCacheProvider>();
        }
    }
}
