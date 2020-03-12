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

            await SavingSemaphoreSlim.WaitAsync(cancellationToken);
            await _fileService
                .SaveTextToFile(Options.FileName, jsonContent, cancellationToken);
            SavingSemaphoreSlim.Release();
            return _fileService.GetFile(Options.FileName);
        }

        private IFile GetFile(string fileName)
        {
            return _fileService.GetFile(fileName);
        }

        private async Task<IDictionary<string,CacheEntryState>> GetItems(IFile file, CancellationToken cancellationToken)
        {
            if(!file.Exists)
                return default;
            await ReadingSemaphoreSlim.WaitAsync(cancellationToken);

            using var fileStream = file.GetFileStream();
            using var streamReader = new StreamReader(fileStream);
            var content = streamReader.ReadToEndAsync();

            ReadingSemaphoreSlim.Release();
            return JsonSerializer.Deserialize<IDictionary<string,CacheEntryState>>(await content);
        }

        public void Dispose()
        {
            
        }
    }
}
