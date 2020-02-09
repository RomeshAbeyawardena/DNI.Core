using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Factories;
using DNI.Shared.Services.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    internal sealed class DefaultExceptionHandlerFactory : IExceptionHandlerFactory
    {
        public static IExceptionHandlerFactory Create(IServiceProvider serviceProvider)
        {
            return new DefaultExceptionHandlerFactory(serviceProvider);
        }

        public IExceptionHandlerFactory RegisterExceptionHandlers(IServiceCollection services, params Assembly[] assemblies)
        {
            var genericServiceType = typeof(IExceptionHandler<>);

            foreach (var assembly in assemblies)
            {
                foreach(var type in assembly.GetTypes().Where(type => type.IsOfType<IExceptionHandler>()))
                {
                    if(!type.IsGenericType)
                        continue;

                    var genericArguments = type.GetGenericArguments();
                    var gServiceType = genericServiceType.MakeGenericType();
                    
                    services.AddSingleton(gServiceType, type);
                }
            }
            return this;
        }

        public bool TryGetExceptionHandler<TException>(out IExceptionHandler<TException> exceptionHandler) where TException : Exception
        {
            exceptionHandler = GetExceptionHandler<TException>();
            return exceptionHandler == null;
        }

        public bool TryGetExceptionHandler(Type exceptionType, out IExceptionHandler exceptionHandler)
        {
            exceptionHandler = GetExceptionHandler(exceptionType);
            return exceptionHandler == null;
        }

        public IExceptionHandler GetExceptionHandler(Type exceptionType)
        {
            var exceptionHandlerType = typeof(IExceptionHandler<>).MakeGenericType(exceptionType);
            return (IExceptionHandler) _serviceProvider.GetService(exceptionHandlerType);
        }

        public IExceptionHandler<TException> GetExceptionHandler<TException>() where TException : Exception
        {
            return _serviceProvider.GetService<IExceptionHandler<TException>>();
        }

        private readonly IServiceProvider _serviceProvider;
        private readonly ISwitch<Type, Type> _exceptionHandlerSwitch;
        private DefaultExceptionHandlerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _exceptionHandlerSwitch = Switch.Create<Type, Type>();
        }
    }
}
