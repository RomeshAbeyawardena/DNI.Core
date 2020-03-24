namespace DNI.Core.Contracts.Stores
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DNI.Core.Contracts.Enumerations;

    public interface ICacheTrackerStore
    {
        Task<IDictionary<string, CacheEntryState>> GetItems(CancellationToken cancellationToken);

        Task<IFile> SaveItems(IDictionary<string, CacheEntryState> state, CancellationToken cancellationToken);
    }
}
