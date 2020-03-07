using DNI.Core.Domains;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Services
{
    public sealed class DefaultValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IValidator<TRequest> _validator;

        public async Task<TResponse> Handle(TRequest request, 
            CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if(_validator == null)
                return await next();

            var response = Activator.CreateInstance<TResponse>();
                
            if(!(response is ResponseBase responseBase))
                return await next();

            var result  = await _validator.ValidateAsync(request);

            if(result.IsValid)
                return await next();

            responseBase.Errors = result.Errors;
            responseBase.IsSuccessful = false;
            return response;
        }

        public DefaultValidationBehaviour(IValidator<TRequest> validator = null)
        {
            _validator = validator;
        }
    }
}
