using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        public static async Task ForEachAsync<T>(this IEnumerable<T> values, 
            Func<T, CancellationToken, Task> forEachItem,
            CancellationToken cancellationToken,
            Func<T, bool> condition = default)
        {
            var items = new List<Task>();
            
            foreach(var value in condition == null 
                ? values 
                : values.Where(condition))
                items.Add(forEachItem(value, cancellationToken));

            await Task.WhenAll(items);
        }

        public static async Task<IEnumerable<T>> ForEachAsync<T>(this IEnumerable<T> values, 
            Func<T, CancellationToken, Task<T>> forEachItem, 
            CancellationToken cancellationToken, 
            Func<T, bool> condition = default)
        {
            var items = new List<Task<T>>();
            
            foreach(var value in condition == null 
                ? values 
                : values.Where(condition))
                items.Add(forEachItem(value, cancellationToken));

            return await Task.WhenAll(items.ToArray());
        }

        public static void ForEach<T>(this IEnumerable<T> values, 
            Action<T> forEachItem, Func<T, bool> condition = default)
        {
            foreach(var value in condition == null ? values : values.Where(condition))
                forEachItem(value);
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> values, 
            Func<T,T> forEachItem, Func<T, bool> condition = default)
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
