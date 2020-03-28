namespace DNI.Core.Services.Abstraction
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using DNI.Core.Contracts;
    using DNI.Core.Domains;
    using DNI.Core.Domains.Contracts;
    using DNI.Core.Services.Attributes;
    using DNI.Core.Services.Exceptions;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [Route("{controller}/{action}")]
    [HandleException]
#pragma warning disable CA1012
    public abstract class DefaultControllerBase : Controller
    {
        protected DefaultControllerBase(IMediator mediator, IMapper mapper)
        {
            Mediator = mediator;
            Mapper = mapper;
        }

        public IMediator Mediator { get; }

        public IMapper Mapper { get; }

        protected void EnsureValidModelState()
        {
            if (!ModelState.IsValid)
            {
                throw new ModelStateException(ModelState);
            }
        }

        /// <summary>
        /// Handles a response.
        /// </summary>
        /// <typeparam name="TResult">Of type TResult.</typeparam>
        /// <param name="response"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onFailure"></param>
        /// <returns></returns>
        protected IActionResult Handle<TResult>(
            IResponse<TResult> response,
            Func<IResponse<TResult>, IActionResult> onSuccess = default,
            Func<IResponse<TResult>, IActionResult> onFailure = default)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if (onSuccess == null)
            {
                onSuccess = response => OnDefaultHandleResponseSuccessful(response);
            }

            if (onFailure == null)
            {
                onFailure = response => OnDefaultHandleResponseFailure(response);
            }

            if (response.IsSuccess)
            {
                return onSuccess(response);
            }

            return onFailure(response);
        }

        /// <summary>
        /// Handles a response asynchronously.
        /// </summary>
        /// <typeparam name="TResult">Of type TResult.</typeparam>
        /// <param name="response"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onFailure"></param>
        /// <returns></returns>
        protected async Task<IActionResult> HandleAsync<TResult>(
            IResponse<TResult> response,
            CancellationToken cancellationToken,
            Func<IResponse<TResult>, CancellationToken, Task<IActionResult>> onSuccess = default,
            Func<IResponse<TResult>, CancellationToken, Task<IActionResult>> onFailure = default)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if (onSuccess == null)
            {
                onSuccess = (response, ct) => OnDefaultHandleResponseSuccessfulAsync(response, ct);
            }

            if (onFailure == null)
            {
                onFailure = (response, ct) => OnDefaultHandleResponseFailureAsync(response, ct);
            }

            if (response.IsSuccess)
            {
                return await onSuccess(response, cancellationToken);
            }

            return await onFailure(response, cancellationToken);
        }

        /// <summary>
        /// Implements a default response handler that is called when a response returns valid.
        /// </summary>
        /// <typeparam name="TResult">Represent the result value type of the <see cref="IResponse{TResult}">response</see> object.</typeparam>
        /// <param name="response">Represents the response object to verify.</param>
        /// <returns></returns>
        protected virtual IActionResult OnDefaultHandleResponseSuccessful<TResult>(IResponse<TResult> response)
        {
            return Ok(response.Result);
        }

        /// <summary>
        /// Implements a default response handler that is called when a response returns invalid.
        /// </summary>
        /// <typeparam name="TResult">Of type TResult.</typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        protected virtual IActionResult OnDefaultHandleResponseFailure<TResult>(IResponse<TResult> response)
        {
            return BadRequest(response.Errors);
        }

        /// <summary>
        /// Implements a default async response handler that is called when a response returns valid.
        /// </summary>
        /// <typeparam name="TResult">Of type TResult.</typeparam>
        /// <param name="response"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual Task<IActionResult> OnDefaultHandleResponseSuccessfulAsync<TResult>(
            IResponse<TResult> response,
            CancellationToken cancellationToken)
        {
            return Task
                .FromResult(OnDefaultHandleResponseSuccessful(response));
        }

        /// <summary>
        /// Implements a default async response handler that is called when a response returns invalid.
        /// </summary>
        /// <typeparam name="TResult">Of type TResult.</typeparam>
        /// <param name="response"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual Task<IActionResult> OnDefaultHandleResponseFailureAsync<TResult>(
            IResponse<TResult> response,
            CancellationToken cancellationToken)
        {
            return Task
                .FromResult(OnDefaultHandleResponseFailure(response));
        }
    }
#pragma warning restore CA1012
}
