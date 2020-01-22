using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Shared.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Append<T>(this IEnumerable<T> items, IEnumerable<T> newItems)
        {
            var itemList = items == null 
                ? new List<T>() 
                : new List<T>(items);

            itemList.AddRange(newItems);
            return itemList.ToArray();
        }
    }
}
