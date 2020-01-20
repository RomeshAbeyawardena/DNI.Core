using System.Collections.Generic;
using System.Text;

namespace DNI.Shared.Shared.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<byte> GetBytes(this string value, Encoding encoding)
        {
            return encoding.GetBytes(value);
        }

        public static string DisplayIf(this string value, bool condition = true, string valueOnConditionFalse = default)
        {
            return condition ? value : valueOnConditionFalse;
        }
    }
}
