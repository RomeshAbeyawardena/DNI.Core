using System;
using System.Reflection;

namespace DNI.Shared.Services.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static object GetDefaultValue(this PropertyInfo propertyInfo)
        {
            var propertyType = propertyInfo.PropertyType;

            if(propertyType == typeof(byte) 
                || propertyType == typeof(short) 
                || propertyType == typeof(int) 
                || propertyType == typeof(long) 
                || propertyType == typeof(decimal)
                || propertyType == typeof(float))
                return 0;

            if(propertyType == typeof(DateTime))
                return default(DateTime);

            if(propertyType == typeof(DateTimeOffset))
                return default(DateTimeOffset);

            if(propertyType == typeof(string))
                return default(string);

            if(propertyType == typeof(Guid))
                return default(Guid);

            if(propertyType == typeof(byte[]))
                return default (byte[]);

            return default;
        }
    }
}
