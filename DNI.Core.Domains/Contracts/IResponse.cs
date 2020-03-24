namespace DNI.Core.Domains.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentValidation.Results;

    public interface IResponse : IResponse<object>
    {
        bool IsSuccessful { get; set; }

        IEnumerable<ValidationFailure> Errors { get; set; }
    }

    public interface IResponse<T>
    {
        T Result { get; set; }
    }
}
