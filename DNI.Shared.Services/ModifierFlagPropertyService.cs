using DNI.Shared.Contracts.Enumerations;
using DNI.Shared.Contracts.Services;
using DNI.Shared.Services.Attributes;
using DNI.Shared.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DNI.Shared.Services
{
    public class ModifierFlagPropertyService : IModifierFlagPropertyService
    {

        public void SetModifierFlagValues<TEntity>(TEntity entity, ModifierFlag modifierFlag)
        {
            var modifierAttributeProperties = GetModifierAttributeProperties<TEntity>();

            var createdModifierFlagAttributes = modifierAttributeProperties
                .Where(a => a.GetCustomAttribute<ModifierAttribute>().ModifierFlag.HasFlag(modifierFlag));

            if (createdModifierFlagAttributes.Any())
                SetModifierFlagValues(createdModifierFlagAttributes, entity, DateTime.Now);

        }

        private IEnumerable<PropertyInfo> GetModifierAttributeProperties<TEntity>()
        {
            var entityType = typeof(TEntity);
            return entityType
                .GetCustomAttributeProperties<ModifierAttribute>(BindingFlags.Public | BindingFlags.Instance);

        }

        private void SetModifierFlagValues<TEntity>(IEnumerable<PropertyInfo> properties, TEntity entity, object value)
        {
            var isDateTime = false;
            var isDateTimeOffset = false;
            DateTimeOffset dateTimeOffset = default;
            DateTime dateTimeValue = default;

            if (value is DateTime _dateTimeValue)
            {
                isDateTime = true;
                dateTimeValue = _dateTimeValue;
            }

            if (value is DateTimeOffset _dateTimeOffset)
            {
                isDateTimeOffset = true;
                dateTimeOffset = _dateTimeOffset;
            }

            if (!isDateTime && !isDateTimeOffset)
                throw new InvalidOperationException();

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(DateTime) && isDateTimeOffset)
                    value = dateTimeOffset.DateTime;

                if (property.PropertyType == typeof(DateTimeOffset) && isDateTime)
                    value = new DateTimeOffset(dateTimeValue);

                if (property.PropertyType == typeof(DateTime?) && isDateTimeOffset)
                    value = dateTimeOffset.DateTime;

                if (property.PropertyType == typeof(DateTimeOffset?) && isDateTime)
                    value = new DateTimeOffset?(dateTimeValue);


                property.SetValue(entity, value);
            }
        }
    }
}
