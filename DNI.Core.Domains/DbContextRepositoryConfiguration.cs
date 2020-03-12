using DNI.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DNI.Core.Services")]
namespace DNI.Core.Domains
{
    public class DbContextRepositoryConfiguration
    {
        public bool UseDbContextPool { get; set; } = true;
        public Type ServiceImplementationType { get; set; }
        public ServiceLifetime ServiceLifetime { get; set; } = ServiceLifetime.Scoped;
        public Action<DbContextOptionsBuilder> DbContextOptions { get; set; } = default;
        public Action<IServiceProvider, DbContextOptionsBuilder> DbContextServiceProviderOptions { get; set; } = default;
        
        internal IEnumerable<Type> EntityTypes => DescribedEntityTypes?.ToTypeArray(); 

        public Action<ITypesDescriptor> EntityTypeDescriber { get; set; }
        
        internal ITypesDescriptor DescribedEntityTypes { get; set; }
    }
}
