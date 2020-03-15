using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Shared.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsDefault(this object value)
        {
            if(value is decimal decValue)
                return decValue == default;

            if(value is long longVal)
                return longVal == default;

            if(value is int intVal)
                return intVal == default;

            if(value is string stringVal)
                return default == stringVal || string.IsNullOrWhiteSpace(stringVal);

            if(value is bool boolValue)
                return default == boolValue;

            return value == default;
        }
    }
}
