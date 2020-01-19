using DNI.Shared.Services.Abstraction;
using System.Reflection;

namespace DNI.Shared.Web
{
    public class ServiceBroker : ServiceBrokerBase
    {
        public ServiceBroker()
        {
            Assemblies = new [] { DefaultAssembly, Assembly.GetAssembly(typeof(ServiceBroker)) };
        }
    }
}
