using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Factories;
using DNI.Shared.Services.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace DNI.Shared.Services
{
    internal sealed class DefaultExceptionHandlerFactory : IExceptionHandlerFactory
    {
        
        public IExceptionHandlerFactory RegisterExceptionHandlers(IServiceCollection services, params Assembly[] assemblies)
        {
            
            foreach (var assembly in assemblies)
            {
                foreach(var type in assembly.GetTypes().Where(type => type.IsOfType<IExceptionHandler>()))
                {
                    var genericServiceType = type.GetInterfaces().SingleOrDefault(interfaceType => interfaceType.IsGenericType);

                    if(genericServiceType == null)
                        continue;

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
            var service = _serviceProvider.GetService(exceptionHandlerType);
            return (IExceptionHandler) service;
        }

        public IExceptionHandler<TException> GetExceptionHandler<TException>() where TException : Exception
        {
            return _serviceProvider.GetService<IExceptionHandler<TException>>();
        }

        private readonly IServiceProvider _serviceProvider;
        public DefaultExceptionHandlerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
    }
}
