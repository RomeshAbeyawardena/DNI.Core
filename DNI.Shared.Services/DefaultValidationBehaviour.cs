﻿using DNI.Shared.Domains;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    public sealed class DefaultValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : ResponseBase
    {
        private readonly IValidator<TRequest> _validator;

        public async Task<TResponse> Handle(TRequest request, 
            CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if(_validator == null)
                return await next();

            var result  = await _validator.ValidateAsync(request);

            if(result.IsValid)
                return await next();

            var response = Activator.CreateInstance<TResponse>();

            response.Errors = result.Errors;
            response.IsSuccessful = false;
            return response;
        }

        public DefaultValidationBehaviour(IValidator<TRequest> validator)
        {
            _validator = validator;
        }
    }
}
