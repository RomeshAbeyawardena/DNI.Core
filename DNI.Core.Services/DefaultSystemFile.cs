using DNI.Core.Contracts;
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

        public string FullPath => System.IO.Path.Combine(Path, Name);

        public bool Exists => File.Exists(FullPath);

        public void Dispose()
        {
            fileStream?.Dispose();
        }

        public FileStream GetFileStream(ILogger logger = default)
        {
            lock (ReadLock)
            {
                return fileStream = RetryHandler.Handle((path) =>
                {
                    if (Exists)
                        return File.Open(path, FileMode.OpenOrCreate);
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
