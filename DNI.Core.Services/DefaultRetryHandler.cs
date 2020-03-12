using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services
{
    public class RetryHandler
    {

    }

    internal sealed class DefaultRetryHandler : IRetryHandler
    {
        public int RetryCount { get; private set; }

        public void Handle(Action handle, int retryAttempts, params Type[] retryExceptions)
        {
            RetryCount = 0;
            try
            {
                handle();
            }
            catch(Exception ex)
            {
                if(retryExceptions.Contains(ex.GetType()) 
                    && RetryCount++ < retryAttempts)
                    Handle(handle, retryAttempts, retryExceptions);
                
                throw;
            }
        }

        public TResult Handle<T, TResult>(Func<T, TResult> handle, T argument, int retryAttempts, params Type[] retryExceptions)
        {
            RetryCount = 0;
            try
            {
                return handle(argument);
            }
            catch(Exception ex)
            {
                if(retryExceptions.Contains(ex.GetType()) 
                    && RetryCount++ < retryAttempts)
                    return Handle(handle, argument, retryAttempts, retryExceptions);
                
                throw;
            }
        }

        public async Task<TResult> Handle<T, TResult>(Func<T, Task<TResult>> handle, T argument, int retryAttempts, params Type[] retryExceptions)
        {
            return await Handle<T, Task<TResult>>(handle, argument, retryAttempts, retryExceptions);
        }

        public async Task Handle<T>(Func<T, Task> handle, T argument, int retryAttempts, params Type[] retryExceptions)
        {
            await Handle<T, Task>(handle, argument, retryAttempts, retryExceptions);
        }
    }
}
