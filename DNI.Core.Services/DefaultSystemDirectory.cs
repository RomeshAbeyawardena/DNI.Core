namespace DNI.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DNI.Core.Contracts;
    using DNI.Core.Contracts.Enumerations;

    internal sealed class DefaultSystemDirectory : IDirectory
    {
        public bool Exists => Directory.Exists(FullPath);

        public string FullPath => System.IO.Path.Combine(Path, Name);

        public string Path { get; }

        public string Name { get; }

        public DirectoryInfo DirectoryInfo => new DirectoryInfo(FullPath);

        public SystemItemType Type => SystemItemType.Directory;

        public DefaultSystemDirectory(string directoryPath)
        {
            Name = System.IO.Path.GetDirectoryName(directoryPath);
            Path = directoryPath.Replace(Name, string.Empty);
        }

        public IEnumerable<IDirectory> GetDirectories(string filter = null)
        {
            var directories = string.IsNullOrWhiteSpace(filter)
                ? Directory.GetDirectories(FullPath, filter)
                : Directory.GetDirectories(FullPath);

            foreach (var directory in directories)
            {
                yield return new DefaultSystemDirectory(directory);
            }
        }

        public IEnumerable<IFile> GetFiles(string filter = null)
        {
            var files = string.IsNullOrWhiteSpace(filter)
                ? Directory.GetFiles(FullPath)
                : Directory.GetFiles(FullPath, filter);

            foreach (var file in files)
            {
                yield return new DefaultSystemFile(file);
            }
        }
    }
}
