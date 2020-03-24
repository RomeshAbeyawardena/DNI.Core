namespace DNI.Core.Shared.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class ObjectExtensions
    {
        public static bool IsDefault(this object value)
        {
            if (value is decimal decValue)
            {
                return decValue == default;
            }

            if (value is long longVal)
            {
                return longVal == default;
            }

            if (value is int intVal)
            {
                return intVal == default;
            }

            if (value is string stringVal)
            {
                return stringVal == default || string.IsNullOrWhiteSpace(stringVal);
            }

            if (value is bool boolValue)
            {
                return boolValue == default;
            }

            if (value is DateTimeOffset dateTimeOffsetValue)
            {
                return dateTimeOffsetValue == default;
            }

            if (value is DateTime dateTimeValue)
            {
                return dateTimeValue == default;
            }

            return value == default;
        }
    }
}
