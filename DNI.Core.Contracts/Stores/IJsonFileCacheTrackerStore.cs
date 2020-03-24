namespace DNI.Core.Contracts.Stores
{
    using System;
    using DNI.Core.Contracts.Options;

    public interface IJsonFileCacheTrackerStore : ICacheTrackerStore
    {
        public IJsonFileCacheTrackerStoreOptions Options { get; }
    }
}
