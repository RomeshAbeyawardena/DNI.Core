# Service Registration
Defines an area containing project specific dependencies, that will be
combined by a user-defined IServiceBroker implementation.
## Usage

    using DNI.Shared.Contracts;
    using Microsoft.Extensions.DependencyInjection;
    
    public class ServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services, 
            IServiceRegistrationOptions options)
        {
            //Register project dependencies here.
            services.AddSingleton<IMyService,MyServiceImplementation>();
        }
    }

