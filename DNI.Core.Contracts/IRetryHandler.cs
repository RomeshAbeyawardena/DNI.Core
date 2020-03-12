using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IRetryHandler
    {
        int RetryCount { get; }
        void Handle(Action handle, int retryAttempts, params Type[] retryExceptions);
        TResult Handle<T, TResult>(Func<T, TResult> handle, T argument, int retryAttempts, params Type[] retryExceptions);
        Task<TResult> Handle<T, TResult>(Func<T, Task<TResult>> handle, T argument, int retryAttempts, params Type[] retryExceptions);
        Task Handle<T>(Func<T, Task> handle, T argument, int retryAttempts, params Type[] retryExceptions);
    }
}
