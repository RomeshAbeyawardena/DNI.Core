using Microsoft.Extensions.DependencyInjection;
using DNI.Shared.Contracts.Options;

namespace DNI.Shared.Contracts
{
    public interface IServiceRegistration
    {
        void RegisterServices(IServiceCollection services, IServiceRegistrationOptions options);
    }
}
