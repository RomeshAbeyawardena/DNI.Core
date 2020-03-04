using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts
{
    public interface IAsyncResultTransformer<T>
    {
        Task<IEnumerable<T>> ArrayAsync();
        Task<IList<T>> ListAsync();
        Task<T> FirstAsync();
        Task<T> FirstOrDefaultAsync();
        Task<T> SingleAsync();
        Task<T> SingleOrDefaultAsync();
    }
}
