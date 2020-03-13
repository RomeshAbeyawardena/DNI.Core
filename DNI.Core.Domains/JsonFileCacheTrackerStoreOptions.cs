using DNI.Core.Contracts.Options;

namespace DNI.Core.Domains
{
    public class JsonFileCacheTrackerStoreOptions : IJsonFileCacheTrackerStoreOptions
    {
        public string FileName { get; set; }
        public int IOExceptionRetryAttempts { get; set; } = 5;
    }
}
