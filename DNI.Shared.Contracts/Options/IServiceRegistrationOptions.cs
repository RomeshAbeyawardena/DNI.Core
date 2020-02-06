using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Options
{
    public interface IServiceRegistrationOptions
    {
        bool RegisterCacheProviders { get; set; }
        bool RegisterMessagePackSerialisers { get; set; }
        bool RegisterAutoMappingProviders { get; set; }
        bool RegisterMediatorServices { get; set; }
    }
}
