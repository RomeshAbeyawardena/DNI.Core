using DNI.Core.Contracts.Enumerations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Providers
{
    /// <summary>
    /// Represents a cache provider that encapsulates multiple caching services
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Gets a value from a specified cache type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheType"></param>
        /// <param name="cacheKeyName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> Get<T>(CacheType cacheType, string cacheKeyName, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Stores a value to a specified cache type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheType"></param>
        /// <param name="cacheKeyName"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Set<T>(CacheType cacheType, string cacheKeyName, T value, CancellationToken cancellationToken = default);

        /// <summary>
        /// Stores a value to a specified cache type using a specified factory method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheType"></param>
        /// <param name="cacheKeyName"></param>
        /// <param name="getValue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> Set<T>(CacheType cacheType, string cacheKeyName, Func<T> getValue, CancellationToken cancellationToken = default);

        /// <summary>
        /// Stores a value to a specified cache type  using a specified async factory method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheType"></param>
        /// <param name="cacheKeyName"></param>
        /// <param name="getValue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> Set<T>(CacheType cacheType, string cacheKeyName, Func<CancellationToken, Task<T>> getValue, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Retrieves an array of values from a specified cache service.
        /// If the value does not exist it stores the result of a specified async factory method and returns the result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheType"></param>
        /// <param name="cacheKeyName"></param>
        /// <param name="getValue"></param>
        /// <param name="append"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetOrSet<T>(CacheType cacheType, string cacheKeyName,
            Func<CancellationToken, Task<IEnumerable<T>>> getValue, bool append = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves an array of values from a specified cache service.
        /// If the value does not exist it stores the result of a specified factory method and returns the result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheType"></param>
        /// <param name="cacheKeyName"></param>
        /// <param name="getValue"></param>
        /// <param name="append"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> GetOrSet<T>(CacheType cacheType, string cacheKeyName, 
            Func<CancellationToken, Task<T>> getValue, bool append = false, CancellationToken cancellationToken = default);
    }
}
