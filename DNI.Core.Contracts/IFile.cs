using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace DNI.Core.Contracts
{
    public interface IFile : ISystemItem, IDisposable
    {
        FileInfo FileInfo { get; } 
        Stream GetFileStream(ILogger logger = default);
    }
}
