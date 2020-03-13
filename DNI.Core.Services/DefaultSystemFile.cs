using DNI.Core.Contracts;
using DNI.Core.Contracts.Enumerations;
using Microsoft.Extensions.Logging;
using System.IO;

namespace DNI.Core.Services
{
    internal sealed class DefaultSystemFile : IFile
    {
        private static readonly object ReadLock = new object();
        
        private FileStream fileStream;
        public string Path { get; }
        public string Name { get; }
        public FileInfo FileInfo => new FileInfo(FullPath);
        public string FullPath => System.IO.Path.Combine(Path, Name);

        public bool Exists => File.Exists(FullPath);

        public SystemItemType Type => SystemItemType.File;

        public void Dispose()
        {
            fileStream?.Dispose();
        }

        public Stream GetFileStream(ILogger logger = default)
        {
            lock (ReadLock)
            {
                return fileStream = RetryHandler.Handle((path) =>
                {
                    if (Exists)
                        return File.Open(path, FileMode.Open);
                    return default;
                }, FullPath, 5, logger, typeof(IOException));
            }

        }

        public DefaultSystemFile(string fileName)
        {
            Name = System.IO.Path.GetFileName(fileName);
            Path = fileName.Replace(Name, string.Empty);
        }
    }
}
