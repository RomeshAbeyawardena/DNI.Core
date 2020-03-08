using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DNI.Core.Services.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<PropertyInfo> GetCustomAttributeProperties<TAttribute>(this Type entityType, BindingFlags bindingFlags)
            where TAttribute : Attribute
        {
            return entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => property.GetCustomAttribute<TAttribute>() != null);
        }

        public static  bool IsOfType<T>(this Type type)
        {
            var ofType = typeof(T);

            return type.GetInterface(ofType.Name) != null;
        }
    }
}
