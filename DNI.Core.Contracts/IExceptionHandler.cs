using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace DNI.Core.Contracts
{
    public interface IExceptionHandler
    {
        bool HandleException(ExceptionContext exception);
    }

    public interface IExceptionHandler<TException> : IExceptionHandler
        where TException : Exception
    {
        
    }
}
