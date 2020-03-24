namespace DNI.Core.Services
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using DNI.Core.Contracts;
    using DNI.Core.Contracts.Options;
    using DNI.Core.Contracts.Services;
    using DNI.Core.Services.Options;
    using Microsoft.Extensions.Logging;

    internal sealed class DefaultFileSystemService : IFileService
    {
        public static SemaphoreSlim SavingSemaphoreSlim = new SemaphoreSlim(1, 1);
        public static SemaphoreSlim ReadingSemaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly ILogger<IFileService> logger;
        private readonly IRetryHandler retryHandler;
        private readonly IRetryHandlerOptions retryHandlerOptions;

        public DefaultFileSystemService(ILogger<IFileService> logger, IRetryHandler retryHandler, IRetryHandlerOptions retryHandlerOptions)
        {
            this.logger = logger;
            this.retryHandler = retryHandler;
            this.retryHandlerOptions = retryHandlerOptions;
        }

        public void Dispose()
        {
            SavingSemaphoreSlim.Dispose();
            ReadingSemaphoreSlim.Dispose();
        }

        public IDirectory GetDirectory(string directoryPath)
        {
            return new DefaultSystemDirectory(directoryPath);
        }

        public IFile GetFile(string fileName)
        {
            return new DefaultSystemFile(fileName);
        }

        public async Task<string> GetTextFromFile(IFile file, CancellationToken cancellationToken)
        {
            try
            {
                await ReadingSemaphoreSlim.WaitAsync(cancellationToken);
                return await retryHandler.Handle(
                    async (file) =>
                {
                    using var fileStream = file.GetFileStream(retryHandlerOptions, logger);

                    using var streamReader = new StreamReader(fileStream);
                    return await streamReader.ReadToEndAsync();
                }, file, retryHandlerOptions.IOExceptionRetryAttempts, false, typeof(IOException));
            }
            finally
            {
                ReadingSemaphoreSlim.Release();
            }
        }

        public async Task SaveTextToFile(IFile file, string content, CancellationToken cancellationToken)
        {
            try
            {
                await SavingSemaphoreSlim.WaitAsync(cancellationToken);
                await retryHandler.Handle(
                    async (file) =>
                {
                    await File.WriteAllTextAsync(file.FullPath, content, cancellationToken);
                }, file, retryHandlerOptions.IOExceptionRetryAttempts, false, typeof(IOException));
            }
            finally
            {
                SavingSemaphoreSlim.Release();
            }
        }
    }
}
