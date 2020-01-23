using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
                || propertyType == typeof(int))
                return 0;

            if(propertyType == typeof(string))
                return default(string);

            if(propertyType == typeof(Guid))
                return default(Guid);

            return default;
        }
    }
}
