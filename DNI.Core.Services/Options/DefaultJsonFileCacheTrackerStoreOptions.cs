namespace DNI.Core.Services.Options
{
    using DNI.Core.Contracts.Options;

    internal class DefaultJsonFileCacheTrackerStoreOptions : IJsonFileCacheTrackerStoreOptions
    {
        public string FileName { get; set; }

        public int IOExceptionRetryAttempts { get; set; }
    }
}
