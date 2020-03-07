using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IMediatorService
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
        Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IRequest<TResponse>;
        Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) 
            where TNotification : INotification;
    }
}
