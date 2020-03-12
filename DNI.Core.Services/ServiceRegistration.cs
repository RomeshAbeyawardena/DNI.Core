using DNI.Core.Contracts;
using DNI.Core.Contracts.Options;
using DNI.Core.Contracts.Convertors;
using DNI.Core.Contracts.Factories;
using DNI.Core.Contracts.Managers;
using DNI.Core.Contracts.Providers;
using DNI.Core.Contracts.Services;
using DNI.Core.Services.Convertors;
using DNI.Core.Services.Factories;
using DNI.Core.Services.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Microsoft.IO;
using MediatR;
using DNI.Core.Services.Generators;
using DNI.Core.Contracts.Generators;
using System.Security.Cryptography;
using DNI.Core.Contracts.Enumerations;
using System;
using System.Reactive.Subjects;
using static Microsoft.IO.RecyclableMemoryStreamManager.Events;
using DNI.Core.Domains.States;
using DNI.Core.Contracts.Stores;
using DNI.Core.Services.Stores;

namespace DNI.Core.Services
{
    public class ServiceRegistration : IServiceRegistration
    {
        private static RecyclableMemoryStreamManager RegisterRecyclableMemoryStreamManager(IServiceProvider serviceProvider)
        {
            var subject = serviceProvider
                .GetService<ISubject<RecyclableMemoryStreamManagerState>>();

            var rMsm = new RecyclableMemoryStreamManager();

            void RMsm_BlockDiscarded()
            {
                subject.OnNext(new RecyclableMemoryStreamManagerState
                {
                    BlockDiscarded = true
                });
            }

            void RMsm_BlockCreated()
            {
                subject.OnNext(new RecyclableMemoryStreamManagerState
                {
                    BlockCreated = true
                });
            }

            void RMsm_UsageReport(long smallPoolInUseBytes, long smallPoolFreeBytes, long largePoolInUseBytes, long largePoolFreeBytes)
            {
                subject.OnNext(new RecyclableMemoryStreamManagerState
                {
                    UsageReportRequested = true,
                    SmallPoolInUseBytes = smallPoolInUseBytes,
                    SmallPoolFreeBytes = smallPoolFreeBytes,
                    LargePoolInUseBytes = largePoolInUseBytes,
                    LargePoolFreeBytes = largePoolFreeBytes
                });
            }

            void RMsm_StreamCreated()
            {
                subject.OnNext(new RecyclableMemoryStreamManagerState
                {
                    StreamCreated = true
                });
            }

            void RMsm_StreamDisposed()
            {
                subject.OnNext(new RecyclableMemoryStreamManagerState
                {
                    StreamDisposed = true
                });
            }

            void RMsm_StreamFinalized()
            {
                subject.OnNext(new RecyclableMemoryStreamManagerState
                {
                    StreamFinalized = true
                });
            }

            void RMsm_LargeBufferCreated()
            {
                subject.OnNext(new RecyclableMemoryStreamManagerState
                {
                    LargeBufferCreated = true
                });
            }

            void RMsm_LargeBufferDiscarded(MemoryStreamDiscardReason reason)
            {
                subject.OnNext(new RecyclableMemoryStreamManagerState
                {
                    LargeBufferDiscarded = true,
                    Reason = reason
                });
            }

            rMsm.BlockCreated += RMsm_BlockCreated;
            rMsm.BlockDiscarded += RMsm_BlockDiscarded;
            rMsm.UsageReport += RMsm_UsageReport;
            rMsm.StreamCreated += RMsm_StreamCreated;
            rMsm.StreamDisposed += RMsm_StreamDisposed;
            rMsm.StreamFinalized += RMsm_StreamFinalized;
            rMsm.LargeBufferCreated += RMsm_LargeBufferCreated;
            rMsm.LargeBufferDiscarded += RMsm_LargeBufferDiscarded;
            return rMsm;
        }



        public void RegisterServices(IServiceCollection services, IServiceRegistrationOptions options)
        {
            services
                .AddSingleton<IFileService, DefaultFileSystemService>()
                .AddSingleton(Switch.Create<CharacterType, Domains.Range>()
                .CaseWhen(CharacterType.Lowercase, new Domains.Range(97, 122))
                .CaseWhen(CharacterType.Uppercase, new Domains.Range(65, 90))
                .CaseWhen(CharacterType.Numerics, new Domains.Range(48, 57))
                .CaseWhen(CharacterType.Symbols, new Domains.Range(33, 47)))
                .AddSingleton<IInstanceServiceInjector, DefaultInstanceServiceInjector>()
                .AddSingleton<IIs, DefaultIs>()
                .AddSingleton(RandomNumberGenerator.Create())
                .AddSingleton<IRandomStringGenerator, DefaultRandomStringGenerator>()
                .AddSingleton<IHttpClientFactory, DefaultHttpClientFactory>()
                .AddSingleton<IGuidService, DefaultGuidService>()
                .AddSingleton<IMarkdownToHtmlService, DefaultMarkdownToHtmlService>()
                .AddSingleton<ISystemClock, SystemClock>()
                .AddSingleton<IClockProvider, DefaultClockProvider>()
                .AddSingleton(typeof(ISubject<>), typeof(Subject<>))
                .AddSingleton(RegisterRecyclableMemoryStreamManager)
                .AddSingleton<IHashingProvider, HashingProvider>()
                .AddSingleton<IClaimTypeValueConvertor, DefaultClaimTypeValueConvertor>()
                .AddSingleton<IModifierFlagPropertyService, DefaultModifierFlagPropertyService>()
                .AddSingleton<IDefaultValueSetterService, DefaultValueSetterService>()
                .AddSingleton<IJsonWebTokenService, DefaultJsonWebTokenService>()
                .AddSingleton<IMemoryStreamManager, DefaultMemoryStreamManager>()
                .AddSingleton<ICryptographyProvider, CryptographyProvider>();

            if (options.RegisterCryptographicProviders)
                services
                    .AddSingleton<IEncryptionProvider, EncryptionProvider>();

            if (options.RegisterMediatorServices)
                services
                    .AddTransient(typeof(IPipelineBehavior<,>), typeof(DefaultValidationBehaviour<,>))
                    .AddTransient<IMediatorService, DefaultMediatorService>();

            if (options.RegisterMessagePackSerialisers)
                services
                    .AddSingleton<IMessagePackService, DefaultMessagePackService>();

            if (options.RegisterAutoMappingProviders)
                services
                    .AddSingleton<IMapperProvider, MapperProvider>();

            if (options.RegisterCacheProviders)
                services
                    .AddScoped(serviceProvider => serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext.Session)
                    .AddScoped<DefaultDistributedCacheService>()
                    .AddScoped<DefaultSessionCacheService>()
                    .AddScoped<ICacheProviderFactory, DefaultCacheProviderFactory>()
                    .AddScoped<ICacheProvider, DefaultCacheProvider>();

            if (options.RegisterExceptionHandlers)
                services.AddSingleton<IExceptionHandlerFactory, DefaultExceptionHandlerFactory>();

            if(options.UseJsonFileCacheTrackerStore)
            {
                services
                    .AddSingleton(options.JsonFileCacheTrackerStoreOptions)
                    .AddSingleton<IJsonFileCacheTrackerStore, DefaultJsonFileCacheTrackerStore>()
                    .AddSingleton<ICacheEntryTracker, DefaultCacheEntryTracker>();
                
            };
        }

    }
}
