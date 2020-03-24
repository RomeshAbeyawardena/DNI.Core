namespace DNI.Core.Contracts
{
    using System;
    using Microsoft.AspNetCore.Mvc.Filters;

    public interface IExceptionHandler
    {
        bool HandleException(ExceptionContext exception);
    }

    public interface IExceptionHandler<TException> : IExceptionHandler
        where TException : Exception
    {
        TException Exception { get; }
    }
}
