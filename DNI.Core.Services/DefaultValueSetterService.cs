namespace DNI.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using DNI.Core.Contracts.Generators;
    using DNI.Core.Contracts.Services;
    using DNI.Core.Services.Extensions;
    using Microsoft.Extensions.DependencyInjection;

    internal sealed class DefaultValueSetterService : IDefaultValueSetterService
    {
        private readonly IServiceProvider serviceProvider;

        public void SetDefaultValues<TEntity>(IEnumerable<PropertyInfo> properties, TEntity value)
        {
            var service = serviceProvider.GetRequiredService<IDefaultValueGenerator<TEntity>>();

            if (service == null)
            {
                return;
            }

            foreach (var property in properties)
            {
                var instance = Instance<object>.Create(() => property.GetValue(value));
                if (instance.Is(val => val == null || val.Equals(property.GetDefaultValue())))
                {
                    property.SetValue(value, service.GetDefaultValue(property.Name, property.PropertyType));
                }
            }
        }

        public void SetDefaultValues<TEntity>(TEntity entity)
        {
            var defaultValueProperties = GetDefaultValueProperties<TEntity>();

            if (defaultValueProperties.Any())
            {
                SetDefaultValues(defaultValueProperties, entity);
            }
        }

        private IEnumerable<PropertyInfo> GetDefaultValueProperties<TEntity>()
        {
            var entityType = typeof(TEntity);
            return entityType.GetCustomAttributeProperties<DefaultValueAttribute>(BindingFlags.Public | BindingFlags.Instance);
        }

        public DefaultValueSetterService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
    }
}
