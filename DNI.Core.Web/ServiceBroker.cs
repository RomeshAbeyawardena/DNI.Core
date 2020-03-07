using DNI.Core.Services.Abstraction;
using System.Reflection;

namespace DNI.Core.Web
{
    public class ServiceBroker : ServiceBrokerBase
    {
        public ServiceBroker()
        {
            DescribeAssemblies = assembliesDescriptor => assembliesDescriptor
                    .GetAssembly<ServiceBroker>();
        }
    }
}
