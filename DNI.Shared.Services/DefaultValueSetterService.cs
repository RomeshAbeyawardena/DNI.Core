using DNI.Shared.Contracts.Generators;
using DNI.Shared.Contracts.Services;
using DNI.Shared.Services.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace DNI.Shared.Services
{
    public class DefaultValueSetterService : IDefaultValueSetterService
    {
        private readonly IServiceProvider _serviceProvider;

        public void SetDefaultValues<TEntity>(IEnumerable<PropertyInfo> properties, TEntity value)
        {
            var service = _serviceProvider.GetRequiredService<IDefaultValueGenerator<TEntity>>();

            if (service == null)
                return;

            foreach (var property in properties)
            {
                var instance = Instance<object>.Create(() => property.GetValue(value));
                if (instance.Is(val => val == null || val.Equals(property.GetDefaultValue())))
                    property.SetValue(value, service.GetDefaultValue(property.Name, property.PropertyType));
            }
        }

        public void SetDefaultValues<TEntity>(TEntity entity)
        {
            var defaultValueProperties = GetDefaultValueProperties<TEntity>();

            if (defaultValueProperties.Any())
                SetDefaultValues(defaultValueProperties, entity);
        }


        private IEnumerable<PropertyInfo> GetDefaultValueProperties<TEntity>()
        {
            var entityType = typeof(TEntity);
            return entityType.GetCustomAttributeProperties<DefaultValueAttribute>(BindingFlags.Public | BindingFlags.Instance);
        }


        public DefaultValueSetterService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
    }
}
