using DNI.Core.Contracts.Options;
using System;

namespace DNI.Core.Contracts.Stores
{
    public interface IJsonFileCacheTrackerStore : ICacheTrackerStore, IDisposable
    {
        public IJsonFileCacheTrackerStoreOptions Options { get; }
    }
}
