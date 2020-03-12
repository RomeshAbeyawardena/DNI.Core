using DNI.Core.Contracts;
using DNI.Core.Contracts.Enumerations;
using DNI.Core.Contracts.Options;
using DNI.Core.Contracts.Services;
using DNI.Core.Contracts.Stores;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Services.Stores
{
    internal sealed class DefaultJsonFileCacheTrackerStore : IJsonFileCacheTrackerStore
    {
        private readonly IFileService _fileService;

        public static SemaphoreSlim SavingSemaphoreSlim = new SemaphoreSlim(1, 1);
        public static SemaphoreSlim ReadingSemaphoreSlim = new SemaphoreSlim(1, 1);

        public DefaultJsonFileCacheTrackerStore(IJsonFileCacheTrackerStoreOptions options, IFileService fileService)
        {
            Options = options;
            _fileService = fileService;
        }

        public IJsonFileCacheTrackerStoreOptions Options { get; }

        public async Task<IDictionary<string, CacheEntryState>> GetItems(CancellationToken cancellationToken)
        {
            using var file = GetFile(Options.FileName);
            return await GetItems(file, cancellationToken);
        }

        public async Task<IFile> SaveItems(IDictionary<string, CacheEntryState> state, CancellationToken cancellationToken)
        {

            var jsonContent = JsonSerializer.Serialize(state);
            try
            {
                await SavingSemaphoreSlim.WaitAsync(cancellationToken);

                return await RetryHandler.Handle(async (fileName) =>
                {
                    await _fileService
                        .SaveTextToFile(fileName, jsonContent, cancellationToken);

                    return _fileService.GetFile(Options.FileName);
                }, Options.FileName, 6, typeof(IOException));

            }
            finally
            {
                SavingSemaphoreSlim.Release();
            }
        }

        private IFile GetFile(string fileName)
        {
            return _fileService.GetFile(fileName);
        }

        private async Task<IDictionary<string, CacheEntryState>> GetItems(IFile file, CancellationToken cancellationToken)
        {
            try
            {
                await ReadingSemaphoreSlim.WaitAsync(cancellationToken);

                if (!file.Exists)
                    return default;

                using var fileStream = file.GetFileStream();
                using var streamReader = new StreamReader(fileStream);
                var content = streamReader.ReadToEndAsync();

                return JsonSerializer.Deserialize<IDictionary<string, CacheEntryState>>(await content);
            }
            finally
            {
                ReadingSemaphoreSlim.Release();
            }
        }

        public void Dispose()
        {

        }
    }
}
