using DNI.Shared.Contracts.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Providers
{
    public interface ICacheProvider
    {
        Task<T> Get<T>(CacheType cacheType, string cacheKeyName, CancellationToken cancellationToken = default);
        Task Set<T>(CacheType cacheType, string cacheKeyName, T value, CancellationToken cancellationToken = default);
        Task<T> Set<T>(CacheType cacheType, string cacheKeyName, Func<T> getValue, CancellationToken cancellationToken = default);
        Task<T> Set<T>(CacheType cacheType, string cacheKeyName, Func<CancellationToken, Task<T>> getValue, CancellationToken cancellationToken = default);
        
        Task<IEnumerable<T>> GetOrSet<T>(CacheType cacheType, string cacheKeyName,
            Func<CancellationToken, Task<IEnumerable<T>>> getValue,  Func<T, object> IdSelector, 
            Func<CancellationToken, Task<object>> getMaxValue,
            bool append = false,
            CancellationToken cancellationToken = default);
        Task<T> GetOrSet<T>(CacheType cacheType, string cacheKeyName, 
            Func<CancellationToken, Task<T>> getValue, bool append = false, CancellationToken cancellationToken = default);
    }
}
