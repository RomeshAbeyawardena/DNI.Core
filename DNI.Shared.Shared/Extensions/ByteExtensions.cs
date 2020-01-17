using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Shared.Extensions
{
    public static class ByteExtensions
    {
        public static string GetString(this IEnumerable<byte> value, Encoding encoding)
        {
            return encoding.GetString(value.ToArray());
        }
    }
}
