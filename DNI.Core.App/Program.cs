using DNI.Core.Services;
using DNI.Core.Services.Abstraction;
using DNI.Core.Services.Extensions;
using System;
using System.Threading.Tasks;

using System.Linq;
using DNI.Core.Contracts;

namespace DNI.Core.App
{
    public static partial class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var serviceBroker = ServiceBrokerBuilder
                .Build(describe => describe.GetAssembly<Startup>());
            
            var value = await AppHost.Build<Startup>()
                .ConfigureServices(services => { ServiceBrokerBuilder
                    .RegisterServiceBroker(services, serviceBroker, options => { 
                        options.RegisterAutoMappingProviders = true; 
                        options.RegisterMessagePackSerialisers = true; 
                    }); 
                }) 
                .ConfigureStartupDelegate((startup, arguments) => startup.Begin(arguments.ToArray()))
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
