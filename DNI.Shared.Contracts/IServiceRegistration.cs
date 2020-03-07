using Microsoft.Extensions.DependencyInjection;
using DNI.Core.Contracts.Options;

namespace DNI.Core.Contracts
{
    public interface IServiceRegistration
    {
        void RegisterServices(IServiceCollection services, IServiceRegistrationOptions options);
    }
}
