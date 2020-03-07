using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNI.Core.Domains
{
    public class ExtendedDbContextOptions : DbContextOptions
    {
        public override Type ContextType => typeof(ExtendedDbContextOptions);

        public override DbContextOptions WithExtension<TExtension>([NotNull] TExtension extension)
        {
            return this;
        }

        public ExtendedDbContextOptions(IReadOnlyDictionary<Type, Microsoft.EntityFrameworkCore.Infrastructure.IDbContextOptionsExtension> extensions)
            : base(extensions)
        {

        }
    }
}
