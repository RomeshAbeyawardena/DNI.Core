using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts
{
    public interface IServiceRegistrationOptions
    {
        bool RegisterCacheProviders { get; set; }
        bool RegisterMessagePackSerialisers { get; set; }
        bool RegisterAutoMappingProviders { get; set; }
    }
}
