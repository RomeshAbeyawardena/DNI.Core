using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IRetryHandler
    {
        TResult Handle<T, TResult>(Func<T, TResult> handle, params Type[] retryExceptions);
        Task<TResult> Handle<T, TResult>(Func<T, Task<TResult>> handle, params Type[] retryExceptions);
        Task Handle<T>(Func<T, Task> handle, params Type[] retryExceptions);
    }
}
