using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IFile : IDisposable
    {
        string FullPath { get; }
        string Path { get; }
        string Name { get; }
        FileStream GetFileStream();
    }
}
