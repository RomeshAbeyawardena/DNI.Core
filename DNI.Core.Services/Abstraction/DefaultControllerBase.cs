using DNI.Core.Contracts;
using DNI.Core.Domains;
using DNI.Core.Services.Attributes;
using DNI.Core.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ResponseHelper = DNI.Core.Domains.Response;

namespace DNI.Core.Services.Abstraction
{
    [Route("{controller}/{action}")]
    [HandleException]
    #pragma warning disable CA1012
    public abstract class DefaultControllerBase : Controller
    {
        protected readonly IMediatorService Mediator;
        protected readonly IMapperProvider Mapper;
        
        public DefaultControllerBase(IMediatorService mediatorService, IMapperProvider mapperProvider)
        {
            Mediator = mediatorService;
            Mapper = mapperProvider;
        }

        protected virtual bool IsResponseValid(ResponseBase response)
        {
            return ResponseHelper.IsSuccessful(response);
        }

        protected async Task<ActionResult> HandleResponse<TResponse>(TResponse response, 
            CancellationToken cancellationToken,
            Func<TResponse, CancellationToken, Task> onSuccess = default,
            Func<TResponse, CancellationToken, Task> onFailure = default)
            where TResponse : ResponseBase
        {
            if(!IsResponseValid(response))
            {
                await onFailure(response, cancellationToken);
                await OnFailure(response, cancellationToken);
                return BadRequest(response.Errors);
            }

            if(onSuccess != null)
                await onSuccess(response, cancellationToken);

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
                return;

            throw new ModelStateException(ModelState);
        }
    }
    #pragma warning restore CA1012
}
