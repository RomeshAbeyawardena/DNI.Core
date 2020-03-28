namespace DNI.Core.Domains
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DNI.Core.Domains.Contracts;
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
    }
}
