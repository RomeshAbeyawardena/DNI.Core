using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Services
{
    public interface IFileService
    {
        IDirectory GetDirectory(string directoryPath);
        IFile GetFile(string fileName);
        Task SaveTextToFile(string fileName, string content, CancellationToken cancellationToken);
    }
}
