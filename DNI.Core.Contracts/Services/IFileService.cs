using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Services
{
    public interface IFileService
    {
        IFile GetFile(string fileName);
        Task SaveTextToFile(string fileName, string content, CancellationToken cancellationToken);
    }
}
