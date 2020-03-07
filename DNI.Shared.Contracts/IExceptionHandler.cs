using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace DNI.Shared.Contracts
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
