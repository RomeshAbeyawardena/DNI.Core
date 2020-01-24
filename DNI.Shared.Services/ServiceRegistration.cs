using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Generators;
using DNI.Shared.Contracts.Managers;
using DNI.Shared.Contracts.Providers;
using DNI.Shared.Services.Generators;
using DNI.Shared.Services.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;

namespace DNI.Shared.Services
{
    public class ServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton(new RecyclableMemoryStreamManager())
                .AddSingleton<IHashingProvider, HashingProvider>()
                .AddSingleton<IMapperProvider, MapperProvider>()
                .AddSingleton<IMemoryStreamManager,MemoryStreamManager>()
                .AddSingleton<ICryptographyProvider, CryptographyProvider>()
                .AddTransient<IMediatorService, MediatorService>();
        }
    }
}
