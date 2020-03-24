namespace DNI.Core.Contracts.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IFileService : IDisposable
    {
        IDirectory GetDirectory(string directoryPath);

        IFile GetFile(string fileName);

        Task SaveTextToFile(IFile file, string content, CancellationToken cancellationToken);

        Task<string> GetTextFromFile(IFile file, CancellationToken cancellationToken);
    }
}
