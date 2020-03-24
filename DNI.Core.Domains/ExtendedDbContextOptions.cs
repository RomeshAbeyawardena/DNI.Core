namespace DNI.Core.Domains
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.EntityFrameworkCore;

    public class ExtendedDbContextOptions : DbContextOptions
    {
        public ExtendedDbContextOptions(IReadOnlyDictionary<Type, Microsoft.EntityFrameworkCore.Infrastructure.IDbContextOptionsExtension> extensions)
            : base(extensions)
        {
            // do
        }

        public override Type ContextType => typeof(ExtendedDbContextOptions);

        public override DbContextOptions WithExtension<TExtension>([NotNull] TExtension extension)
        {
            return this;
        }
    }
}
