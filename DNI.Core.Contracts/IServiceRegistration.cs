namespace DNI.Core.Contracts
{
    using DNI.Core.Contracts.Options;
    using Microsoft.Extensions.DependencyInjection;

    public interface IServiceRegistration
    {
        void RegisterServices(IServiceCollection services, IServiceRegistrationOptions options);
    }
}
