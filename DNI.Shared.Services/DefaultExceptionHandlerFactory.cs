using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    internal sealed class DefaultExceptionHandlerFactory : IExceptionHandlerFactory
    {
        public static IExceptionHandlerFactory Create(Action<IExceptionHandlerFactory> configure)
        {
            var factory = new DefaultExceptionHandlerFactory();
            configure(factory);
            return factory;
        }
        public IExceptionHandlerFactory AddExceptionHandler<TException>(IExceptionHandler exceptionHandler, params Type[] handledexceptions) where TException : Exception
        {
            return AddExceptionHandler(typeof(TException), exceptionHandler, handledexceptions);
        }

        public IExceptionHandlerFactory AddExceptionHandler(Type exceptionType, IExceptionHandler exceptionHandler, params Type[] handledexceptions)
        {
            _exceptionHandlerSwitch.CaseWhen(exceptionType, exceptionHandler, handledexceptions);
            return this;
        }

        public IExceptionHandler GetExceptionHandler(Type exceptionType)
        {
            return _exceptionHandlerSwitch.Case(exceptionType);
        }

        public IExceptionHandler GetExceptionHandler<TException>() where TException : Exception
        {
            return GetExceptionHandler(typeof(TException));
        }

        public bool TryGetExceptionHandler<TException>(out IExceptionHandler exceptionHandler) where TException : Exception
        {
            return TryGetExceptionHandler(typeof(TException), out exceptionHandler);
        }

        public bool TryGetExceptionHandler(Type exceptionType, out IExceptionHandler exceptionHandler)
        {
            exceptionHandler = GetExceptionHandler(exceptionType);

            return exceptionHandler != null;
        }

        private readonly ISwitch<Type, IExceptionHandler> _exceptionHandlerSwitch;
        private DefaultExceptionHandlerFactory()
        {
            _exceptionHandlerSwitch = Switch.Create<Type, IExceptionHandler>();
        }
    }
}
