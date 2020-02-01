using DNI.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Options
{
    public class DefaultServiceRegistrationOptions : IServiceRegistrationOptions
    {
        public static IServiceRegistrationOptions DefaultsOptions => new DefaultServiceRegistrationOptions { 
            RegisterAutoMappingProviders = true, 
            RegisterMessagePackSerialisers = true 
        };

        public bool RegisterCacheProviders { get; set; }
        public bool RegisterMessagePackSerialisers { get; set; }
        public bool RegisterAutoMappingProviders { get; set; }
    }
}
