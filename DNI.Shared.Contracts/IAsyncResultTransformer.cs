using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts
{
    public interface IAsyncResultTransformer<T>
    {
        Task<IEnumerable<T>> ArrayAsync(CancellationToken cancellationToken);
        Task<decimal?> SumAsync(Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken);
        Task<int> CountAsync(CancellationToken cancellationToken);
        Task<long> LongCountAsync(CancellationToken cancellationToken);
        Task<IList<T>> ListAsync(CancellationToken cancellationToken);
        Task<T> FirstAsync(CancellationToken cancellationToken);
        Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken);
        Task<T> SingleAsync(CancellationToken cancellationToken);
        Task<T> SingleOrDefaultAsync(CancellationToken cancellationToken);
    }
}
