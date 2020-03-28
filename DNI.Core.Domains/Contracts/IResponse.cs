namespace DNI.Core.Domains.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentValidation.Results;

    /// <summary>
    /// Represents a response with a result and a list of errors.
    /// </summary>
    /// <typeparam name="TResult">Result Type.</typeparam>
    public interface IResponse<TResult>
    {
        /// <summary>
        /// Gets or sets a value indicating whether result was successful.
        /// </summary>
        bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the actual result.
        /// </summary>
        TResult Result { get; set; }

        /// <summary>
        /// Gets or sets a value indicating a list of errors that occured.
        /// </summary>
        IEnumerable<ValidationFailure> Errors { get; set; }

        /// <summary>
        /// Gets a value indicating whether <see cref="TResult">result</see> was successful and not null.
        /// </summary>
        public bool IsSuccess { get; }
    }
}
