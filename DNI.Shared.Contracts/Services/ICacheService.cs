using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Services
{
    public interface ICacheService
    {
        Task<T> Get<T>(string cacheKeyName, CancellationToken cancellationToken = default);
        Task Set<T>(string cacheKeyName, T value, CancellationToken cancellationToken = default);
        Task<T> Set<T>(string cacheKeyName, Func<T> getValue, CancellationToken cancellationToken = default);
        Task<T> Set<T>(string cacheKeyName, Func<Task<T>> getValue, CancellationToken cancellationToken = default);
    }
}
