using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;

namespace DNI.Core.Shared.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<byte> GetBytes(this string value, Encoding encoding)
        {
            return encoding.GetBytes(value);
        }

        public static string ToBase64String(this string value, Encoding encoding)
        {
            return Convert.ToBase64String(value.GetBytes(encoding).ToArray());
        }

        public static string FromBase64String(this string value, Encoding encoding)
        {
            return Convert.FromBase64String(value).GetString(encoding);
        }

        public static string DisplayIf(this string value, bool condition = true, string valueOnConditionFalse = default)
        {
            return condition ? value : valueOnConditionFalse;
        }

    }
}
