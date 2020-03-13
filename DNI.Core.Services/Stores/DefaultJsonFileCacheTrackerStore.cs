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
        private readonly IJsonSerializer _jsonSerializer;

        public DefaultJsonFileCacheTrackerStore(IJsonFileCacheTrackerStoreOptions options,
            IFileService fileService, IJsonSerializer jsonSerializer)
        {
            Options = options;
            _fileService = fileService;
            _jsonSerializer = jsonSerializer;

        }

        public IJsonFileCacheTrackerStoreOptions Options { get; }

        public async Task<IDictionary<string, CacheEntryState>> GetItems(CancellationToken cancellationToken)
        {
            using var file = GetFile(Options.FileName);
            return await GetItems(file, cancellationToken);
        }

        public async Task<IFile> SaveItems(IDictionary<string, CacheEntryState> state, CancellationToken cancellationToken)
        {
            if (state == null || state.Count < 1)
                return default;

            var jsonContent = JsonSerializer.Serialize(state);

            var file = GetFile(Options.FileName);

            await _fileService.SaveTextToFile(file, jsonContent, cancellationToken);

            return file;

        }

        private IFile GetFile(string fileName)
        {
            return _fileService.GetFile(fileName);
        }

        private async Task<IDictionary<string, CacheEntryState>> GetItems(IFile file, CancellationToken cancellationToken)
        {
            if (!file.Exists)
                return default;

            var content = await _fileService.GetTextFromFile(file, cancellationToken);

            if (string.IsNullOrWhiteSpace(content))
                return new Dictionary<string, CacheEntryState>();

            return _jsonSerializer.Deserialize<IDictionary<string, CacheEntryState>>(content);

        }

    }
}
