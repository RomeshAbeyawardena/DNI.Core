using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts
{
    public interface IMediatorService
    {
        Task<TResponse> Send<TResponse, TRequest>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IRequest<TResponse>;
        Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) 
            where TNotification : INotification;
    }
}
