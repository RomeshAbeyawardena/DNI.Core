using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Factories
{
    public interface IExceptionHandlerFactory
    {
        IExceptionHandler GetExceptionHandler(Type exceptionType);
        IExceptionHandler GetExceptionHandler<TException>()
            where TException : Exception;
        IExceptionHandlerFactory AddExceptionHandler<TException>(IExceptionHandler exceptionHandler);
        IExceptionHandlerFactory AddExceptionHandler(Type exceptionType, IExceptionHandler exceptionHandler);
    }
}
