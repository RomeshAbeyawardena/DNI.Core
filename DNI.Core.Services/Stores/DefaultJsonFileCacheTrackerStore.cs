using DNI.Core.Contracts;
using DNI.Core.Contracts.Enumerations;
using DNI.Core.Contracts.Options;
using DNI.Core.Contracts.Services;
using DNI.Core.Contracts.Stores;
using Microsoft.Extensions.Logging;
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
        private readonly IRetryHandler _retryHandler;
        public static SemaphoreSlim SavingSemaphoreSlim = new SemaphoreSlim(1, 1);
        public static SemaphoreSlim ReadingSemaphoreSlim = new SemaphoreSlim(1, 1);

        public DefaultJsonFileCacheTrackerStore(ILogger<IJsonFileCacheTrackerStore> logger, IJsonFileCacheTrackerStoreOptions options, 
            IFileService fileService, IRetryHandler retryHandler)
        {
            _logger = logger;
            Options = options;
            _fileService = fileService;
            _retryHandler = retryHandler;
        }

        private readonly ILogger<IJsonFileCacheTrackerStore> _logger;

        public IJsonFileCacheTrackerStoreOptions Options { get; }

        public async Task<IDictionary<string, CacheEntryState>> GetItems(CancellationToken cancellationToken)
        {
            using var file = GetFile(Options.FileName);
            return await GetItems(file, cancellationToken);
        }

        public async Task<IFile> SaveItems(IDictionary<string, CacheEntryState> state, CancellationToken cancellationToken)
        {
            if(state == null || state.Count < 1)
                return default;

            var jsonContent = JsonSerializer.Serialize(state);
            try
            {
                await SavingSemaphoreSlim.WaitAsync(cancellationToken);

                return await _retryHandler.Handle(async (fileName) =>
                {
                    await _fileService
                        .SaveTextToFile(fileName, jsonContent, cancellationToken);

                    return _fileService.GetFile(Options.FileName);
                }, Options.FileName, Options.IOExceptionRetryAttempts, false, typeof(IOException));

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

                string content = string.Empty;

                if (!file.Exists)
                    return default;

                using (var fileStream = file.GetFileStream(_logger))
                { 
                    using var streamReader = new StreamReader(fileStream);
                    content = await streamReader.ReadToEndAsync();
                }
                if(string.IsNullOrWhiteSpace(content))
                    return new Dictionary<string, CacheEntryState>();

                return JsonSerializer.Deserialize<IDictionary<string, CacheEntryState>>(content);
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
