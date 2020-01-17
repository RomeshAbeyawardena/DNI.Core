using DNI.Shared.Services;
using DNI.Shared.Services.Abstraction;
using DNI.Shared.Services.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DNI.Shared.App
{
    public static class Program
    {
        public static async Task Main()
        {
            var serviceCollection = new ServiceCollection();

            //serviceCollection.RegisterServiceBroker<ServiceBroker>();

            var booleanSwitch = Switch.Create<string, bool>()
                .CaseWhen("YES", true, "yes", "1")
                .CaseWhen("NO", false, "no", "0");

            var case1 = booleanSwitch.Case("YES");
            var case2 = booleanSwitch.Case("NO");
            var case3 = booleanSwitch.Case("1");
            var case4 = booleanSwitch.Case("0");
            var case5 = booleanSwitch.Case("A");

            var mClassArray = new [] { new MClass { K1 = 12456, V1 = "htfhd" } };
            
            var test = booleanSwitch["YES"];

            var myDict = DictionaryBuilder.Create<int, string>()
                .AddRange(mClassArray, mClass => mClass.K1, mClass => mClass.V1)
                .ToDictionary();

            var myList = ListBuilder.Create<int>()
                .AddRange(mClassArray, mClass => mClass.K1);

            Try.Create()
                .Try(() => throw new FieldAccessException())
                .Catch<FieldAccessException>(OnCatch)
                .Invoke();

             await Try.CreateAsync()
                    .Try(async() => {  await Task.Delay(2000); throw new TimeoutException(); })
                    .Try(async() => await Task.Delay(2000))
                    .Try(async() => await Task.Delay(2000))
                    .Catch<TimeoutException>(OnCatch)
                    .InvokeAsync();

            var results = await Try.CreateAsync<int, int>()
                .Try(async (a) => await Task.FromResult(a + 5))
                .Try(async (a) => await Task.FromResult(a + 10))
                .Try(async (a) => await Task.FromResult(a + 15))
                .InvokeAsync(5);
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
    }
}
