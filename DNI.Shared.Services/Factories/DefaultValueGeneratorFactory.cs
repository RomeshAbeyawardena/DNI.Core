using DNI.Shared.Contracts.Factories;
using DNI.Shared.Contracts.Generators;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Factories
{
    public sealed class DefaultValueGeneratorFactory : IDefaultValueGeneratorFactory
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
