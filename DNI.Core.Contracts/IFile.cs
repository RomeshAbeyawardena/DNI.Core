using System;
using System.IO;

namespace DNI.Core.Contracts
{
    public interface IFile : IDisposable
    {
        bool Exists { get; }
        string FullPath { get; }
        string Path { get; }
        string Name { get; }
        FileStream GetFileStream();
    }
}
