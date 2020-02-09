using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Factories
{
    public interface IExceptionHandlerFactory
    {
        bool TryGetExceptionHandler<TException>(out IExceptionHandler exceptionHandler)
            where TException : Exception;
        bool TryGetExceptionHandler(Type exceptionType, out IExceptionHandler exceptionHandler);
        IExceptionHandler GetExceptionHandler(Type exceptionType);
        IExceptionHandler GetExceptionHandler<TException>()
            where TException : Exception;
        IExceptionHandlerFactory AddExceptionHandler<TException>(IExceptionHandler exceptionHandler, params Type[] handledexceptions)
            where TException : Exception;
        IExceptionHandlerFactory AddExceptionHandler(Type exceptionType, IExceptionHandler exceptionHandler, params Type[] handledexceptions);
    }
}
