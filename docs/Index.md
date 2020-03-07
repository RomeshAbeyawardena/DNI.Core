# DNI Shared Library

## Usage
### Installation
    Install-Package DNI.Core.Services
### Implementation
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

## Table of contents
- Dependency Management
  - [IServiceBroker](ServiceBroker/index.md)
    - Defines a broker used to interact with IServiceRegistration instances 
referenced throughout your project.
  - [IServiceRegistration](ServiceRegistration/index.md)
    -  Defines a subset of services defined within a project, enabling isolation 
of projects.
  - [Extensions](DependencyManagement/extensions.md)
    - Extension methods used to support dependency management.
 - EntityFramework
   - [DbContextBase](EntityFramework/DbContextBase.md)
     - Extended functionality implemented to the DbContextBase to support
the following new features:
        - [IModifier](Attributes/Modifier/index.md) flag properties 
(Created and Modified flags on Add and update)
        - [Default value](Attributes/DefaultValue/index.md) properties