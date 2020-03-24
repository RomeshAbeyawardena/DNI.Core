namespace DNI.Core.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using DNI.Core.Domains;
    using FluentValidation;
    using MediatR;

    public sealed class DefaultValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IValidator<TRequest> validator;

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (validator == null)
            {
                return await next();
            }

            var response = Activator.CreateInstance<TResponse>();

            if (!(response is ResponseBase responseBase))
            {
                return await next();
            }

            var result = await validator.ValidateAsync(request);

            if (result.IsValid)
            {
                return await next();
            }

            responseBase.Errors = result.Errors;
            responseBase.IsSuccessful = false;
            return response;
        }

        public DefaultValidationBehaviour(IValidator<TRequest> validator = null)
        {
            this.validator = validator;
        }
    }
}
