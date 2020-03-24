namespace DNI.Core.Services.Factories
{
    using System;
    using DNI.Core.Contracts.Factories;
    using DNI.Core.Contracts.Generators;
    using Microsoft.Extensions.DependencyInjection;

    internal sealed class DefaultValueGeneratorFactory : IDefaultValueGeneratorFactory
    {
        private readonly IServiceProvider serviceProvider;

        public IDefaultValueGenerator<TEntity> GetDefaultValueGenerator<TEntity>()
        {
            return serviceProvider.GetRequiredService<IDefaultValueGenerator<TEntity>>();
        }

        public DefaultValueGeneratorFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
    }
}
