using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Managers;
using DNI.Shared.Contracts.Providers;
using DNI.Shared.Services.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
