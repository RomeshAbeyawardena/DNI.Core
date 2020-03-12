using DNI.Core.Contracts;
using DNI.Core.Contracts.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Services
{
    internal sealed class DefaultFileSystemService : IFileService
    {
        public IFile GetFile(string fileName)
        {
            return new DefaultSystemFile(fileName);
        }

        public async Task SaveTextToFile(string fileName, string content, CancellationToken cancellationToken)
        {
            var file = GetFile(fileName);

            await File.WriteAllTextAsync(file.FullPath, content, cancellationToken);
        }
    }
}
