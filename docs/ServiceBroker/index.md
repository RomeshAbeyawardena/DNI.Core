# Service Broker

## Usage
Inherit from DNI.Core.Services.Abstraction.ServiceBrokerBase, 
instead of IServiceBroker to automatically inherit built-in services. 

It's recommended to abstract this class into its own project to reduce 
decoupling.

    using DNI.Core.Services.Abstraction;
    public class ServiceBroker : ServiceBrokerBase
    {
        public ServiceBroker()
        {
            //List all assemblies with implemented 
            //IServiceRegistration classes 
            Assemblies = new [] { DefaultAssembly, 
                    Assembly.GetAssembly(typeof(ServiceBroker)) };
        }
    }

An implemented ServiceBrokerBase will automatically register some 
internal services using a built-in IServiceRegistration implementation.

    services
        .AddSingleton(Switch.Create<CharacterType, Domains.Range>()
            .CaseWhen(CharacterType.Lowercase, new Domains.Range(97, 122))
            .CaseWhen(CharacterType.Uppercase, new Domains.Range(65, 90))
            .CaseWhen(CharacterType.Numerics, new Domains.Range(48, 57))
            .CaseWhen(CharacterType.Symbols, new Domains.Range(33, 47)))
        .AddSingleton(RandomNumberGenerator.Create())
        .AddSingleton<IRandomStringGenerator, 
                        DefaultRandomStringGenerator>()
        .AddSingleton<IHttpClientFactory, DefaultHttpClientFactory>()
        .AddSingleton<IGuidService, DefaultGuidService>()
        .AddSingleton<IMarkdownToHtmlService, 
                        DefaultMarkdownToHtmlService>()
        .AddSingleton<ISystemClock, SystemClock>()
        .AddSingleton<IClockProvider, DefaultClockProvider>()
        .AddSingleton(new RecyclableMemoryStreamManager())
        .AddSingleton<IHashingProvider, HashingProvider>()
        .AddSingleton<IClaimTypeValueConvertor, 
                        DefaultClaimTypeValueConvertor>()
        .AddSingleton<IModifierFlagPropertyService, 
                        DefaultModifierFlagPropertyService>()
        .AddSingleton<IDefaultValueSetterService, 
                        DefaultValueSetterService>()
        .AddSingleton<IJsonWebTokenService, DefaultJsonWebTokenService>()
        .AddSingleton<IMemoryStreamManager, DefaultMemoryStreamManager>()
        .AddSingleton<ICryptographyProvider, CryptographyProvider>()
        .AddSingleton<IEncryptionProvider,EncryptionProvider>();

### Implementation

To register the service broker to your startup.cs, use the extension helper

    services.RegisterServiceBroker<TServiceBroker>(configure => {
        [configuration of IServiceRegistrationOptions]});

### IServiceRegistrationOptions

The default service registration options object supports the following options:

- RegisterCacheProviders 

  - When set to true will execute the following:
         
        services
            .AddScoped(serviceProvider => serviceProvider
                .GetRequiredService<IHttpContextAccessor>()
                    .HttpContext.Session)
            .AddScoped<DefaultDistributedCacheService>()
            .AddScoped<DefaultSessionCacheService>()
            .AddScoped<ICacheProviderFactory, 
                DefaultCacheProviderFactory>()
            .AddScoped<ICacheProvider, DefaultCacheProvider>();
        

- RegisterMessagePackSerialisers 
  - When set to true will execute the following:
  
        services.AddSingleton<IMessagePackService, 
            DefaultMessagePackService>()
- RegisterAutoMappingProviders 
  - When set to true will execute the following:
        
        services.AddSingleton<IMapperProvider, MapperProvider>();

- RegisterMediatorServices 
  - When set to true will execute the following:
        
        services.AddTransient(typeof(IPipelineBehavior<,>), 
                    typeof(DefaultValidationBehaviour<,>))
                    .AddTransient<IMediatorService, 
                        DefaultMediatorService>();
- RegisterExceptionHandlers 
  - When set to true will trigger the scanning of IExceptionHandler instances
and execute the following:
        
        
            if(options.RegisterExceptionHandlers)
                services.AddSingleton<IExceptionHandlerFactory, 
                    DefaultExceptionHandlerFactory>();
