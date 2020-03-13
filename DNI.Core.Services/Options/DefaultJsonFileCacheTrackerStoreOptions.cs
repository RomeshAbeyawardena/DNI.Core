using DNI.Core.Contracts.Options;

namespace DNI.Core.Services.Options
{
    internal class DefaultJsonFileCacheTrackerStoreOptions : IJsonFileCacheTrackerStoreOptions
    {
        public string FileName { get; set; }
        public int IOExceptionRetryAttempts { get; set; }
    }
}
