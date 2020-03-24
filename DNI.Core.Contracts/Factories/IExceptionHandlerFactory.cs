namespace DNI.Core.Contracts.Factories
{
    using System;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;

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
