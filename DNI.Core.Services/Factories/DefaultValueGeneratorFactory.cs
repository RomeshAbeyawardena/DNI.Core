using DNI.Core.Contracts.Factories;
using DNI.Core.Contracts.Generators;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DNI.Core.Services.Factories
{
    internal sealed class DefaultValueGeneratorFactory : IDefaultValueGeneratorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public IDefaultValueGenerator<TEntity> GetDefaultValueGenerator<TEntity>()
        {
            return _serviceProvider.GetRequiredService<IDefaultValueGenerator<TEntity>>();
        }

        public DefaultValueGeneratorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
    }
}
