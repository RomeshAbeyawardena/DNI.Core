using DNI.Shared.Services;
using DNI.Shared.Services.Abstraction;
using DNI.Shared.Services.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DNI.Shared.App
{
    public static class Program
    {
        public static void Main()
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
