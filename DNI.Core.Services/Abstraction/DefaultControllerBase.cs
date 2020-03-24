namespace DNI.Core.Services.Abstraction
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DNI.Core.Contracts;
    using DNI.Core.Domains;
    using DNI.Core.Services.Attributes;
    using DNI.Core.Services.Exceptions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using ResponseHelper = DNI.Core.Domains.Response;

    [Route("{controller}/{action}")]
    [HandleException]
#pragma warning disable CA1012
    public abstract class DefaultControllerBase : Controller
    {
        public DefaultControllerBase(IMediatorService mediatorService, IMapperProvider mapperProvider)
        {
            Mediator = mediatorService;
            Mapper = mapperProvider;
        }

        protected IMediatorService Mediator { get; }

        protected IMapperProvider Mapper { get; }

        protected virtual bool IsResponseValid(ResponseBase response)
        {
            return ResponseHelper.IsSuccessful(response);
        }

        protected async Task InvokeOnNotNull<TResponse>(
            TResponse response,
            CancellationToken cancellationToken,
            Func<TResponse, CancellationToken, Task> onNotNull = default)
        {
            if (onNotNull != null)
            {
                await onNotNull(response, cancellationToken);
            }
        }

        protected async Task<ActionResult> HandleResponse<TResponse>(
            TResponse response,
            CancellationToken cancellationToken,
            Func<TResponse, CancellationToken, Task> onSuccess = default,
            Func<TResponse, CancellationToken, Task> onFailure = default)
            where TResponse : ResponseBase
        {
            if (!IsResponseValid(response))
            {
                await InvokeOnNotNull(response, cancellationToken, onFailure);

                await OnFailure(response, cancellationToken);

                return BadRequest(response.Errors);
            }

            await InvokeOnNotNull(response, cancellationToken, onSuccess);

            await OnSuccess(response, cancellationToken);

            return Ok(response.Result);
        }

        protected virtual Task OnSuccess(ResponseBase responseBase, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnFailure(ResponseBase responseBase, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected void EnsureModalStateIsValid()
        {
            if (ModelState.IsValid)
            {
                return;
            }

            throw new ModelStateException(ModelState);
        }
    }
#pragma warning restore CA1012
}
