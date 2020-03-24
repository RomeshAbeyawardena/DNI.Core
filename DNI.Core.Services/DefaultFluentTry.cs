namespace DNI.Core.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DNI.Core.Contracts;
    using DNI.Core.Domains;

    internal class DefaultFluentTry : IFluentTry
    {
        private readonly ConcurrentBag<Action> actionConcurrentBag;
        private readonly ISwitch<Type, ExceptionHandler> catchActionSwitch;

        public IFluentTry Catch<TException>(Action<Exception> exceptionAction, bool continueOnExceptionThrow = false)
        {
            catchActionSwitch
                .CaseWhen(typeof(TException), ExceptionHandler.Create(exceptionAction, continueOnExceptionThrow));
            return this;
        }

        public void Invoke()
        {
            foreach (var action in actionConcurrentBag)
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception exception)
                {
                    if (!HandleException(exception, out var continueOperation))
                    {
                        throw;
                    }

                    if (!continueOperation)
                    {
                        break;
                    }
                }
            }
        }

        public IFluentTry Try(Action action)
        {
            actionConcurrentBag.Add(action);
            return this;
        }

        public static IFluentTry Create()
        {
            return new DefaultFluentTry(new ConcurrentBag<Action>(), Switch.Create<Type, ExceptionHandler>());
        }

        protected bool HandleException(Exception exception, out bool continueOnHandledException)
        {
            continueOnHandledException = false;
            var exceptionAction = catchActionSwitch
                .Case(exception.GetType());

            if (exceptionAction == null)
            {
                return false;
            }

            continueOnHandledException = exceptionAction.ContinueOnExceptionThrow;

            exceptionAction.ExceptionAction.Invoke(exception);
            return true;
        }

        protected DefaultFluentTry(ConcurrentBag<Action> actionConcurrentBag, ISwitch<Type, ExceptionHandler> catchActionSwitch)
        {
            this.actionConcurrentBag = actionConcurrentBag;
            this.catchActionSwitch = catchActionSwitch;
        }
    }

    internal class DefaultFluentTry<TResult> : DefaultFluentTry, IFluentTry<TResult>
    {
        private readonly ConcurrentBag<Func<TResult>> actionConcurrentBag;

        public virtual new IEnumerable<TResult> Invoke()
        {
            var resultConcurrentBag = new ConcurrentBag<TResult>();
            try
            {
                foreach (var result in GetResults(actionConcurrentBag, (a) => a()))
                {
                    resultConcurrentBag.Add(result);
                }
            }
            catch (Exception exception)
            {
                if (!HandleException(exception, out var continueOperation))
                {
                    throw;
                }
            }

            return resultConcurrentBag;
        }

        public IFluentTry<TResult> Try(Func<TResult> action)
        {
            actionConcurrentBag.Add(action);
            return this;
        }

        public static new IFluentTry<TResult> Create()
        {
            return new DefaultFluentTry<TResult>(new ConcurrentBag<Func<TResult>>(), Switch.Create<Type, ExceptionHandler>());
        }

        public new IFluentTry<TResult> Catch<TException>(Action<Exception> exceptionAction, bool continueOnExceptionThrow = false)
        {
            return (IFluentTry<TResult>)base.Catch<TException>(exceptionAction, continueOnExceptionThrow);
        }

        protected IEnumerable<TResult> GetResults<TDelegate>(ConcurrentBag<TDelegate> delegateConcurrentBag, Func<TDelegate, TResult> getResult)
            where TDelegate : Delegate
        {
            var resultConcurrentBag = new ConcurrentBag<TResult>();

            foreach (var action in delegateConcurrentBag)
            {
                try
                {
                    resultConcurrentBag.Add(getResult(action));
                }
                catch (Exception exception)
                {
                    if (!HandleException(exception, out var continueOnHandledException))
                    {
                        throw;
                    }

                    if (!continueOnHandledException)
                    {
                        break;
                    }
                }
            }
            return resultConcurrentBag;
        }

        protected DefaultFluentTry(ConcurrentBag<Func<TResult>> actionConcurrentBag, ISwitch<Type, ExceptionHandler> catchActionSwitch)
            : base(null, catchActionSwitch)
        {
            this.actionConcurrentBag = actionConcurrentBag;
        }
    }

    internal class DefaultFluentTry<T, TResult> : DefaultFluentTry<TResult>, IFluentTry<T, TResult>
    {
        private readonly ConcurrentBag<Func<T, TResult>> actionConcurrentBag;

        public new static IFluentTry<T, TResult> Create()
        {
            return new DefaultFluentTry<T, TResult>(new ConcurrentBag<Func<T, TResult>>(), Switch.Create<Type, ExceptionHandler>());
        }

        public IEnumerable<TResult> Invoke(T value)
        {
            var resultConcurrentBag = new ConcurrentBag<TResult>();
            try
            {
                foreach (var item in GetResults(actionConcurrentBag, a => a.Invoke(value)))
                {
                    resultConcurrentBag.Add(item);
                }
            }
            catch (Exception exception)
            {
                if (!HandleException(exception, out var continueOperation))
                {
                    throw;
                }
            }

            return resultConcurrentBag;
        }

        public IFluentTry<T, TResult> Try(Func<T, TResult> result)
        {
            actionConcurrentBag.Add(result);
            return this;
        }

        public new IFluentTry<T, TResult> Catch<TException>(Action<Exception> exceptionAction, bool continueOnExceptionThrown = false)
        {
            return (IFluentTry<T, TResult>)base.Catch<TException>(exceptionAction, continueOnExceptionThrown);
        }

        protected DefaultFluentTry(ConcurrentBag<Func<T, TResult>> actionConcurrentBag, ISwitch<Type, ExceptionHandler> catchActionSwitch)
            : base(null, catchActionSwitch)
        {
            this.actionConcurrentBag = actionConcurrentBag;
        }
    }

    internal class DefaultFluentTryAsync : DefaultFluentTry<Task>, IFluentTryAsync
    {
        public new static IFluentTryAsync Create()
        {
            return new DefaultFluentTryAsync(new ConcurrentBag<Func<Task>>(), Switch.Create<Type, ExceptionHandler>());
        }

        public async Task InvokeAsync()
        {
            try
            {
                await Task.WhenAll(Invoke()).ConfigureAwait(false); ;
            }
            catch (Exception exception)
            {
                if (!HandleException(exception, out var continueOperation))
                {
                    throw;
                }
            }
        }

        public new IFluentTryAsync Catch<TException>(Action<Exception> exceptionAction, bool continueOnExceptionThrown = false)
        {
            return (IFluentTryAsync)base.Catch<TException>(exceptionAction, continueOnExceptionThrown);
        }

        public new IFluentTryAsync Try(Func<Task> result)
        {
            return (IFluentTryAsync)base.Try(result);
        }

        private DefaultFluentTryAsync(ConcurrentBag<Func<Task>> actionConcurrentBag, ISwitch<Type, ExceptionHandler> catchActionSwitch)
            : base(actionConcurrentBag, catchActionSwitch)
        {
        }
    }

    internal class DefaultFluentTryAsync<TResult> : DefaultFluentTry<Task<TResult>>, IFluentTryAsync<TResult>
    {
        public new static IFluentTryAsync<TResult> Create()
        {
            return new DefaultFluentTryAsync<TResult>(new ConcurrentBag<Func<Task<TResult>>>(), Switch.Create<Type, ExceptionHandler>());
        }

        public new IFluentTryAsync<TResult> Catch<TException>(Action<Exception> exceptionAction, bool continueOnExceptionThrown)
        {
            return (IFluentTryAsync<TResult>)base.Catch<TException>(exceptionAction, continueOnExceptionThrown);
        }

        public new IFluentTryAsync<TResult> Try(Func<Task<TResult>> result)
        {
            return (IFluentTryAsync<TResult>)base.Try(result);
        }

        public async Task<IEnumerable<TResult>> InvokeAsync()
        {
            return await Task.WhenAll(Invoke());
        }

        private DefaultFluentTryAsync(
            ConcurrentBag<Func<Task<TResult>>> actionConcurrentBag,
            ISwitch<Type, ExceptionHandler> catchActionSwitch)
            : base(actionConcurrentBag, catchActionSwitch)
        {
        }
    }

    internal class DefaultFluentTryAsync<T, TResult> : DefaultFluentTry<T, Task<TResult>>, IFluentTryAsync<T, TResult>
    {
        public DefaultFluentTryAsync(ConcurrentBag<Func<T, Task<TResult>>> actionConcurrentBag, ISwitch<Type, ExceptionHandler> catchActionSwitch)
            : base(actionConcurrentBag, catchActionSwitch)
        {
        }

        public async Task<IEnumerable<TResult>> InvokeAsync(T value)
        {
            try
            {
                return await Task.WhenAll(Invoke(value)).ConfigureAwait(false); ;
            }
            catch (Exception exception)
            {
                if (!HandleException(exception, out var continueOperation))
                {
                    throw;
                }
            }

            return default;
        }

        public new IFluentTryAsync<T, TResult> Catch<TException>(Action<Exception> exceptionAction, bool continueOnExceptionThrown = false)
        {
            return (IFluentTryAsync<T, TResult>)base.Catch<TException>(exceptionAction);
        }

        public new IFluentTryAsync<T, TResult> Try(Func<T, Task<TResult>> result)
        {
            return (IFluentTryAsync<T, TResult>)base.Try(result);
        }

        public new static IFluentTryAsync<T, TResult> Create()
        {
            return new DefaultFluentTryAsync<T, TResult>(new ConcurrentBag<Func<T, Task<TResult>>>(), Switch.Create<Type, ExceptionHandler>());
        }
    }
}
