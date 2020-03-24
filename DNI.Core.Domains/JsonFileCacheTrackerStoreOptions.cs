namespace DNI.Core.Domains
{
    using DNI.Core.Contracts.Options;

    public class JsonFileCacheTrackerStoreOptions : IJsonFileCacheTrackerStoreOptions
    {
        public string FileName { get; set; }
    }
}
