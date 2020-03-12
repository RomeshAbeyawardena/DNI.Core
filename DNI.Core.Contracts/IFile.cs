using System;
using System.IO;

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
