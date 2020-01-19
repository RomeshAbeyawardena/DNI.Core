using DNI.Shared.Services;
using DNI.Shared.Services.Abstraction;
using DNI.Shared.Services.Extensions;
using System;
using System.Threading.Tasks;

using System.Linq;
using DNI.Shared.Contracts;

namespace DNI.Shared.App
{
    public static partial class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var value = await AppHost.Build<Startup>()
                .Configure(appHost => appHost.OnStart += AppHost_OnStart)
                .ConfigureServices(services => services.RegisterServiceBroker<ServiceBroker>())
                .ConfigureStartupDelegate((startup, args) => startup.Begin(args.ToArray()))
                .Start<int>(args).ConfigureAwait(false);

            return value;
        }

        private static void AppHost_OnStart(object sender, IAppHostEventArgs e)
        {
            Console.WriteLine("Arguments: ",string.Join(',', e.Arguments));
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
