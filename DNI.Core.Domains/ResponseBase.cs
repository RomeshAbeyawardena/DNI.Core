namespace DNI.Core.Domains
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DNI.Core.Domains.Contracts;
    using DNI.Core.Shared.Extensions;
    using FluentValidation.Results;
    using Newtonsoft.Json;

    public abstract class ResponseBase<TResult> : IResponse<TResult>
    {
        /// <inheritdoc/>
        public bool IsSuccessful { get; set; }

        /// <inheritdoc/>
        public TResult Result { get; set; }

        /// <inheritdoc/>
        public IEnumerable<ValidationFailure> Errors { get; set; }

        /// <inheritdoc/>
        public bool IsSuccess => this.Result != null && this.IsSuccessful;

        public static TResponse Success<TResponse>(TResult result)
            where TResponse : IResponse<TResult>
        {
            return Activator.CreateInstance<TResponse>().Configure(response =>
            {
                var responseBase = response as ResponseBase<TResult>;
                responseBase.IsSuccessful = true;
                responseBase.Result = result;
                responseBase.Errors = Array.Empty<ValidationFailure>();
            });
        }

        public static TResponse Failed<TResponse>(IEnumerable<ValidationFailure> errors)
            where TResponse : IResponse<TResult>
        {
            return Activator.CreateInstance<TResponse>().Configure(response =>
            {
                var responseBase = response as ResponseBase<TResult>;
                responseBase.IsSuccessful = false;
                responseBase.Result = default;
                responseBase.Errors = errors;
            });
        }
    }
}
