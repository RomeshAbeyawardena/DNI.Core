namespace DNI.Core.Services.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using DNI.Core.Contracts.Generators;
    using DNI.Core.Services.Generators;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.DependencyInjection;

    internal class ExtendedDbContextOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {
        public override bool IsDatabaseProvider => false;

        public override string LogFragment => nameof(ExtendedDbContextOptionsExtensionInfo);

        public override long GetServiceProviderHashCode()
        {
            return 0;
        }

        public override void PopulateDebugInfo([NotNull] IDictionary<string, string> debugInfo)
        {
            throw new NotImplementedException();
        }

        public ExtendedDbContextOptionsExtensionInfo(ExtendedDbContextOptionsExtension extendedDbContextOptionsExtension)
            : base(extendedDbContextOptionsExtension)
        {
        }
    }

    public class ExtendedDbContextOptionsExtension : IDbContextOptionsExtension
    {
        public DbContextOptionsExtensionInfo Info => new ExtendedDbContextOptionsExtensionInfo(this);

        public void ApplyServices(IServiceCollection services)
        {
            services
                .AddSingleton(typeof(IDefaultValueGenerator<>), typeof(DefaultValueGenerator<>));
        }

        public void Validate(IDbContextOptions options)
        {
        }
    }
}
