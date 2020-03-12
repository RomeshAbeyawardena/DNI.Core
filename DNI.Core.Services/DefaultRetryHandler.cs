using DNI.Core.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Humanizer;

namespace DNI.Core.Services
{
    public static class RetryHandler
    {
        public static void Handle(Action handle, int retryAttempts, 
            ILogger logger = default, params Type[] retryExceptions)
        {
            new DefaultRetryHandler(logger)
                .Handle(handle, retryAttempts, false, retryExceptions);
        }

        public static TResult Handle<T, TResult>(Func<T, TResult> handle, T argument, int retryAttempts, 
            ILogger logger = default, params Type[] retryExceptions)
        {
            return new DefaultRetryHandler(logger)
                .Handle(handle, argument, retryAttempts, false, retryExceptions);
        }

        public static async Task<TResult> Handle<T, TResult>(Func<T, Task<TResult>> handle, T argument, int retryAttempts, 
            ILogger logger = default, params Type[] retryExceptions)
        {
            return await new DefaultRetryHandler(logger)
                .Handle(handle, argument, retryAttempts, false, retryExceptions);
        }

        public static async Task Handle<T>(Func<T, Task> handle, T argument, int retryAttempts, 
            ILogger logger = default, params Type[] retryExceptions)
        {
            await new DefaultRetryHandler(logger)
                .Handle(handle, argument, retryAttempts, false, retryExceptions);
        }
    }

    internal sealed class DefaultRetryHandler : IRetryHandler
    {
        private readonly ILogger _logger;
        private void HandleException(Exception ex, int maximumAttempts)
        {
            var timeoutInSeconds = Timeout / 1000;
            _logger?.LogWarning(ex, "Attempt {0} of {1}: Failed and handled within a retry handler, retrying in {2} {3}", 
                RetryCount, maximumAttempts, timeoutInSeconds, timeoutInSeconds > 0 ? "seconds" : "second");
            Thread.Sleep(Timeout);
        }

        private bool IsExceptionHandled(Exception exception, int retryAttempts, params Type[] retryExceptions)
        {
            var exceptionType = exception.GetType();
            return retryExceptions.Contains(exceptionType)
                    && RetryCount++ < retryAttempts;
        }

        public DefaultRetryHandler(ILogger logger = null)
        {
            _logger = logger;
        }

        public int RetryCount { get; private set; }
        public int Timeout => RetryCount * 1000;
        public void Handle(Action handle, int retryAttempts, bool isRetry = false,  params Type[] retryExceptions)
        {
            if(!isRetry)
                RetryCount = 0;

            try
            {
                handle();
            }
            catch (Exception ex)
            {
                if (IsExceptionHandled(ex, retryAttempts, retryExceptions))
                {
                    HandleException(ex, retryAttempts);
                    Handle(handle, retryAttempts, true, retryExceptions);
                }
                throw;
            }
        }

        public TResult Handle<T, TResult>(Func<T, TResult> handle, T argument, int retryAttempts, bool isRetry = false, params Type[] retryExceptions)
        {
            if(!isRetry)
                RetryCount = 0;

            try
            {
                return handle(argument);
            }
            catch (Exception ex)
            {
                if (IsExceptionHandled(ex, retryAttempts, retryExceptions))
                {
                    HandleException(ex, retryAttempts);
                    return Handle(handle, argument, retryAttempts, true, retryExceptions);
                }
                throw;
            }
        }

        public async Task<TResult> Handle<T, TResult>(Func<T, Task<TResult>> handle, T argument, int retryAttempts, bool isRetry = false, params Type[] retryExceptions)
        {
            try
            {
                return await Handle<T, Task<TResult>>(handle, argument, retryAttempts, true, retryExceptions);
            }
            catch (Exception ex)
            {
                if (IsExceptionHandled(ex, retryAttempts, retryExceptions))
                {
                    HandleException(ex, retryAttempts);
                    return await Handle(handle, argument, retryAttempts, true, retryExceptions);
                }
                throw;
            }

        }

        public async Task Handle<T>(Func<T, Task> handle, T argument, int retryAttempts, bool isRetry = false, params Type[] retryExceptions)
        {
            try
            {
                await Handle<T, Task>(handle, argument, retryAttempts, true, retryExceptions);
            }
            catch (Exception ex)
            {
                if (IsExceptionHandled(ex, retryAttempts, retryExceptions))
                {
                    HandleException(ex, retryAttempts);
                    await Handle(handle, argument, retryAttempts, true, retryExceptions);
                }
                throw;
            }

        }
    }
}
