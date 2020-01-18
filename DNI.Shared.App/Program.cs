using DNI.Shared.Services;
using DNI.Shared.Services.Abstraction;
using DNI.Shared.Services.Extensions;
using DNI.Shared.Services.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Threading.Tasks;

using System.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace DNI.Shared.App
{
    public static partial class Program
    {
        public static async Task Main()
        {
            await new DefaultAppHost<Startup>()
                .ConfigureServices(services => services.RegisterServiceBroker<ServiceBroker>())
                .ConfigureStartupDelegate((startup, args) => startup.Begin(args.ToArray()))
                .Start();
        }

        public static void OnCatch(Exception ex)
        {
            Console.WriteLine(ex);
        }


        class MClass
        {
            public int K1 { get; set; }
            public string V1 { get; set; }
        }

        public class ServiceBroker : ServiceBrokerBase
        {
            public ServiceBroker()
            {
                Assemblies = new [] { DefaultAssembly, GetAssembly<ServiceBroker>() };
            }
        }

        public class MyDisposable : IDisposable
        {
            public string Name { get; }

            public void Dispose()
            {
                Dispose(true);
            }

            public int GetNumber()
            {
                return 5;
            }

            public async Task<int> GetNumberAsync()
            {
                return await Task.FromResult(GetNumber());
            }

            public void CallMe()
            {
                Console.WriteLine($"{ Name } CallMe was invoked.");
            }

            public async Task CallMeAsync()
            {
                await Task.Delay(1000);
                Console.WriteLine($"{ Name } CallMe was invoked async.");
            }

            protected virtual void Dispose(bool gc)
            {
                if(gc)
                    Console.WriteLine($"Then { Name } was disposed");
            }

            public MyDisposable(string name)
            {
                Name = name;
            }
        }
    }
}
