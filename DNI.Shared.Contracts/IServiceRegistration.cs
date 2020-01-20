using Microsoft.Extensions.DependencyInjection;

namespace DNI.Shared.Contracts
{
    public interface IServiceRegistration
    {
        void RegisterServices(IServiceCollection serviceCollection);
    }
}
