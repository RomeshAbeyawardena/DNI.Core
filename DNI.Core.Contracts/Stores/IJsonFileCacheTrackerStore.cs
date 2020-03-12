using DNI.Core.Contracts.Options;

namespace DNI.Core.Contracts.Stores
{
    public interface IJsonFileCacheTrackerStore : ICacheTrackerStore
    {
        public IJsonFileCacheTrackerStoreOptions Options { get; }
    }
}
