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
        public static void Handle(Action handle, int retryAttempts, params Type[] retryExceptions)
        {
            new DefaultRetryHandler()
                .Handle(handle, retryAttempts, retryExceptions);
        }

        public static TResult Handle<T, TResult>(Func<T, TResult> handle, T argument, int retryAttempts, params Type[] retryExceptions)
        {
            return new DefaultRetryHandler()
                .Handle(handle, argument, retryAttempts, retryExceptions);
        }

        public static async Task<TResult> Handle<T, TResult>(Func<T, Task<TResult>> handle, T argument, int retryAttempts, params Type[] retryExceptions)
        {
            return await new DefaultRetryHandler()
                .Handle(handle, argument, retryAttempts, retryExceptions);
        }

        public static async Task Handle<T>(Func<T, Task> handle, T argument, int retryAttempts, params Type[] retryExceptions)
        {
            await new DefaultRetryHandler()
                .Handle(handle, argument, retryAttempts, retryExceptions);
        }
    }

    internal sealed class DefaultRetryHandler : IRetryHandler
    {
        private readonly ILogger<IRetryHandler> _logger;
        private void HandleException(Exception ex)
        {
            var timeoutInSeconds = Timeout / 1000;
            _logger?.LogWarning(ex, "Failed and handled within retry handler, retrying in {0} {1}", 
                timeoutInSeconds, timeoutInSeconds > 0 ? "seconds" : "second");
            Thread.Sleep(Timeout);
        }

        public DefaultRetryHandler(ILogger<IRetryHandler> logger = null)
        {
            _logger = logger;
        }

        public int RetryCount { get; private set; }
        public int Timeout => RetryCount * 1000;
        public void Handle(Action handle, int retryAttempts, params Type[] retryExceptions)
        {
            RetryCount = 0;
            try
            {
                handle();
            }
            catch (Exception ex)
            {
                if (retryExceptions.Contains(ex.GetType())
                    && RetryCount++ < retryAttempts)
                {
                    HandleException(ex);
                    Handle(handle, retryAttempts, retryExceptions);
                }
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
            catch (Exception ex)
            {
                if (retryExceptions.Contains(ex.GetType())
                    && RetryCount++ < retryAttempts)
                {
                    HandleException(ex);
                    return Handle(handle, argument, retryAttempts, retryExceptions);
                }
                throw;
            }
        }

        public async Task<TResult> Handle<T, TResult>(Func<T, Task<TResult>> handle, T argument, int retryAttempts, params Type[] retryExceptions)
        {
            try
            {
                return await Handle<T, Task<TResult>>(handle, argument, retryAttempts, retryExceptions);
            }
            catch (Exception ex)
            {
                if (retryExceptions.Contains(ex.GetType())
                    && RetryCount++ < retryAttempts)
                {
                    HandleException(ex);
                    return await Handle(handle, argument, retryAttempts, retryExceptions);
                }
                throw;
            }

        }

        public async Task Handle<T>(Func<T, Task> handle, T argument, int retryAttempts, params Type[] retryExceptions)
        {
            try
            {
                await Handle<T, Task>(handle, argument, retryAttempts, retryExceptions);
            }
            catch (Exception ex)
            {
                if (retryExceptions.Contains(ex.GetType())
                    && RetryCount++ < retryAttempts)
                {
                    HandleException(ex);
                    await Handle(handle, argument, retryAttempts, retryExceptions);
                }
                throw;
            }

        }
    }
}
