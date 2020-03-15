using System;
using System.Collections.Generic;
using System.Linq;

namespace DNI.Core.Shared.Extensions
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

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> values, Func<T,T> forEachItem, Func<T, bool> condition = default)
        {
            var items = new List<T>();
            
            foreach(var value in condition == null 
                ? values 
                : values.Where(condition))
                items.Add(forEachItem(value));

            return items.ToArray();
        }
    }
}
