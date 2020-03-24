namespace DNI.Core.Contracts
{
    using System;
    using System.IO;
    using DNI.Core.Contracts.Options;
    using Microsoft.Extensions.Logging;

    public interface IFile : ISystemItem, IDisposable
    {
        FileInfo FileInfo { get; }

        Stream GetFileStream(IRetryHandlerOptions options = default, ILogger logger = default);
    }
}
