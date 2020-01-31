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
        Task Set<T>(T value, CancellationToken cancellationToken = default);
        Task Set<T>(Func<T> getValue, CancellationToken cancellationToken = default);
    }
}
