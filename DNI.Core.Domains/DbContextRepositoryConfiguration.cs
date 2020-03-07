using DNI.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Domains
{
    public class DbContextRepositoryConfiguration
    {
        public bool UseDbContextPool { get; set; } = true;
        public Type ServiceImplementationType { get; set; }
        public ServiceLifetime ServiceLifetime 
            { get; set; } = ServiceLifetime.Scoped;
        public Action<DbContextOptionsBuilder> DbContextOptions 
            { get; set; } = default;
        public Action<IServiceProvider, DbContextOptionsBuilder> DbContextServiceProviderOptions 
            { get; set; } = default;
        
        public IEnumerable<Type> EntityTypes => DescribedEntityTypes.ToTypeArray(); 
        public ITypesDescriptor DescribedEntityTypes { get; set; }
    }
}
