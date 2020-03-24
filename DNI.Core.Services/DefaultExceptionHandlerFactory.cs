namespace DNI.Core.Services
{
    using System;
    using System.Linq;
    using System.Reflection;
    using DNI.Core.Contracts;
    using DNI.Core.Contracts.Factories;
    using DNI.Core.Services.Extensions;
    using Microsoft.Extensions.DependencyInjection;

    internal sealed class DefaultExceptionHandlerFactory : IExceptionHandlerFactory
    {
        public IExceptionHandlerFactory RegisterExceptionHandlers(IServiceCollection services, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes().Where(type => type.IsOfType<IExceptionHandler>()))
                {
                    var genericServiceType = type.GetInterfaces().SingleOrDefault(interfaceType => interfaceType.IsGenericType);

                    if (genericServiceType == null)
                    {
                        continue;
                    }

                    services.AddSingleton(genericServiceType, type);
                }
            }
            return this;
        }

        public bool TryGetExceptionHandler<TException>(out IExceptionHandler<TException> exceptionHandler) where TException : Exception
        {
            exceptionHandler = GetExceptionHandler<TException>();
            return exceptionHandler != null;
        }

        public bool TryGetExceptionHandler(Type exceptionType, out IExceptionHandler exceptionHandler)
        {
            exceptionHandler = GetExceptionHandler(exceptionType);
            return exceptionHandler != null;
        }

        public IExceptionHandler GetExceptionHandler(Type exceptionType)
        {
            var exceptionHandlerType = typeof(IExceptionHandler<>).MakeGenericType(exceptionType);
            var service = serviceProvider.GetService(exceptionHandlerType);
            return (IExceptionHandler)service;
        }

        public IExceptionHandler<TException> GetExceptionHandler<TException>() where TException : Exception
        {
            return serviceProvider.GetService<IExceptionHandler<TException>>();
        }

        private readonly IServiceProvider serviceProvider;

        public DefaultExceptionHandlerFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
    }
}
