using System;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Services
{
    public interface ICacheService
    {
        Task<T> Get<T>(string cacheKeyName, CancellationToken cancellationToken = default);
        Task Set<T>(string cacheKeyName, T value, CancellationToken cancellationToken = default);
        Task<T> Set<T>(string cacheKeyName, Func<T> getValue, CancellationToken cancellationToken = default);
        Task<T> Set<T>(string cacheKeyName, Func<CancellationToken, Task<T>> getValue, CancellationToken cancellationToken = default);
    }
}
