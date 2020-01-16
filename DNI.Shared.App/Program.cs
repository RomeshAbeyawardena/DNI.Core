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
                .CaseWhen("YES", true, "1")
                .CaseWhen("No", false, "0");

            var case1 = booleanSwitch.Case("YES");
            var case2 = booleanSwitch.Case("NO");
            var case3 = booleanSwitch.Case("1");
            var case4 = booleanSwitch.Case("0");
            var case5 = booleanSwitch.Case("A");
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
