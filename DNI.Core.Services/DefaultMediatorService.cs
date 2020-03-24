namespace DNI.Core.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using DNI.Core.Contracts;
    using MediatR;

    internal sealed class DefaultMediatorService : IMediatorService
    {
        private readonly IMediator mediator;

        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            await mediator.Publish(notification, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            return await mediator.Send(request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IRequest<TResponse>
        {
            return await mediator.Send(request, cancellationToken).ConfigureAwait(false);
        }

        public DefaultMediatorService(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}
