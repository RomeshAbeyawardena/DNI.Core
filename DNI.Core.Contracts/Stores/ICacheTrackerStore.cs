using DNI.Core.Contracts.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Stores
{
    public interface ICacheTrackerStore
    {
        Task<IDictionary<string, CacheEntryState>> GetItems(CancellationToken cancellationToken);

        Task<IFile> SaveItems(IDictionary<string, CacheEntryState> state, CancellationToken cancellationToken);
    }
}
