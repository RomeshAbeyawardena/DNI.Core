using DNI.Shared.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    public class MediatorService : IMediatorService
    {
        private readonly IMediator _mediator;

        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) 
            where TNotification : INotification
        {
            await _mediator.Publish(notification, cancellationToken);
        }

        public async Task<TResponse> Send<TResponse, TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest<TResponse>
        {
            return await _mediator.Send(request, cancellationToken);
        }

        public MediatorService(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
