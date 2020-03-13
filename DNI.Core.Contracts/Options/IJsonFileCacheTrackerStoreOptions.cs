namespace DNI.Core.Contracts.Options
{
    public interface IJsonFileCacheTrackerStoreOptions
    {
        string FileName { get; set; }
        int IOExceptionRetryAttempts {  get; set; }
    }
}
