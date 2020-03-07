# DNI Shared Library

## Getting Started

    Install-Package DNI.Core.Services

**Startup.cs**
    
    using DNI.Core.Services;
    using DNI.Core.Services.Abstraction;

    //TODO: Move into own class/project

    public class ServiceBroker : ServiceBrokerBase
    {
        // Include all assemblies that contain 
        // IServiceRegistration instances
        
        Assemblies = new [] { DefaultAssembly, Assembly
                    .GetAssembly(typeof(ServiceBroker)) };
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .RegisterServiceBroker<ServiceBroker>(options => { 
                    options.RegisterAutoMappingProviders = true; 
                    options.RegisterMessagePackSerialisers = true;
                    options.RegisterCacheProviders = true;
                    options.RegisterMediatorServices = true;
                    options.RegisterExceptionHandlers = true; }, 
                out var serviceBroker)
        }
    }


## Documentation

Find Technical Documentation [here](/docs/Index.md)
