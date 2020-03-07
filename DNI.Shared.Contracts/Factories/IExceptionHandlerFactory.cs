using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace DNI.Shared.Contracts.Factories
{
    public interface IExceptionHandlerFactory
    {
        bool TryGetExceptionHandler<TException>(out IExceptionHandler<TException> exceptionHandler)
            where TException : Exception;
        
        bool TryGetExceptionHandler(Type exceptionType, out IExceptionHandler exceptionHandler);

        IExceptionHandler GetExceptionHandler(Type exceptionType);

        IExceptionHandler<TException> GetExceptionHandler<TException>()
            where TException : Exception;

        IExceptionHandlerFactory RegisterExceptionHandlers(IServiceCollection services, params Assembly[] assemblies);
    }
}
