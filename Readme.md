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
        
        Assemblies = new [] { DefaultAssembly,
                    .GetAssembly<ServiceBroker>()) };
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

### Extend functionality of DbContext in Entity Framework Core

Reasons to use this instead of the standard DbContext class
- Inherits directly from the DbContext class, providing all standard features available
to the DbContext
- Adds ability to singularise table names using Humanizer
- Adds ability to use ModifierFlag attributes on data models to 
automatically populate default data on insertion and updates
- Adds ability to supply default values at a code level, to prevent
unexpected null values reaching the database layer.

**CmsDbContext.cs**
    
    using DNI.Core.Services.Abstraction;

    public class CmsDbContext : DbContextBase
    {
        public CmsDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions, 
                    true/false, //Toggles singularise table names functionality
                    true/false, //Toggles modifier flag attributes functionality
                    true/false, //Toggles default value functionality)
        {
            //additional configuration here.
        }

        //Add DbSet classes here
    }
    
**DataServiceRegistration.cs**

    using DNI.Core.Contracts;

    public class DataServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services, 
                IServiceRegistrationOptions options)
        {
            services
                .RegisterDbContextRepositories<CmsDbContext>(config => {
                    config.UseDbContextPool = true/false,
                        //only use if using own IRepository service 
                        //implementation
                        //config.ServiceImplementationType = 
                                    typeof(MyRepository) 
                        config.ServiceLifetime = ServiceLifetime
                                            .Scoped/Transient;
                        //config.DbContextOptions => config
                            .UseSqlServer(connString);
                        config.DbContextServiceProviderOptions => config
                                        .UseSqlServer(connString);
                        config.DescribedEntityTypes = TypeDescriptor
                                            .Describe<Customer>()
                                            .Describe<Applicant>();
            });
        }
    }

## Documentation

Find Technical Documentation [here](/docs/Index.md)
