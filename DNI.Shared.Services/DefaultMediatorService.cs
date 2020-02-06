using DNI.Shared.Contracts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    internal sealed class DefaultMediatorService : IMediatorService
    {
        private readonly IMediator _mediator;

        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) 
            where TNotification : INotification
        {
            await _mediator.Publish(notification, cancellationToken).ConfigureAwait(false);;
        }

        public async Task<TResponse> Send<TResponse, TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest<TResponse>
        {
            return await _mediator.Send(request, cancellationToken).ConfigureAwait(false);;
        }

        public DefaultMediatorService(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
