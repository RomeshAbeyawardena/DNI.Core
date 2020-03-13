using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IDirectory : ISystemItem
    {
        DirectoryInfo DirectoryInfo { get; }
        IEnumerable<IDirectory> GetDirectories(string filter = default);
        IEnumerable<IFile> GetFiles(string filter = default);
    }
}
