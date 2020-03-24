namespace DNI.Core.Contracts.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represent a cache service to retrieve and store data from cache.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Retrieves a cached item of T from cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKeyName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> Get<T>(string cacheKeyName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Stores a value to cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKeyName"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Set<T>(string cacheKeyName, T value, CancellationToken cancellationToken = default);

        /// <summary>
        /// Stores a result of a factory method to cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKeyName"></param>
        /// <param name="getValue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> Set<T>(string cacheKeyName, Func<T> getValue, CancellationToken cancellationToken = default);

        /// <summary>
        /// Stores a result of an async factory method to cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKeyName"></param>
        /// <param name="getValue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> Set<T>(string cacheKeyName, Func<CancellationToken, Task<T>> getValue, CancellationToken cancellationToken = default);
    }
}
