using DNI.Core.Contracts.Options;
using System;

namespace DNI.Core.Contracts.Stores
{
    public interface IJsonFileCacheTrackerStore : ICacheTrackerStore
    {
        public IJsonFileCacheTrackerStoreOptions Options { get; }
    }
}
