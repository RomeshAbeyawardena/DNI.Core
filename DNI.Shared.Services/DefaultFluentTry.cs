using DNI.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    internal class DefaultFluentTry : IFluentTry
    {
        private IList<Action> _actionList;
        private ISwitch<Type, Action<Exception>> _catchActionSwitch;

        public IFluentTry Catch<TException>(Action<Exception> exceptionAction)
        {
            _catchActionSwitch
                .CaseWhen(typeof(TException), exceptionAction);
            return this;
        }

        public void Invoke()
        {
            try
            {
                foreach(var action in _actionList)
                    action.Invoke();
            }
            catch(Exception exception)
            {
                if(!HandleException(exception))
                    throw;
            }
        }

        public IFluentTry Try(Action action)
        {
            _actionList.Add(action);
            return this;
        }

        public static IFluentTry Create()
        {
            return new DefaultFluentTry(new List<Action>(), Switch.Create<Type, Action<Exception>>());
        }

        protected bool HandleException(Exception exception)
        {
            var exceptionAction = _catchActionSwitch.Case(exception.GetType());

            if(exceptionAction == null)
                return false;

            exceptionAction.Invoke(exception);
            return true;
        }

        protected DefaultFluentTry(IList<Action> actionList, ISwitch<Type, Action<Exception>> catchActionSwitch)
        {
            _actionList = actionList;
            _catchActionSwitch = catchActionSwitch;
        }
    }

    internal class DefaultFluentTry<TResult> : DefaultFluentTry, IFluentTry<TResult>
    {
        private IList<Func<TResult>> _actionList;

        public virtual new IEnumerable<TResult> Invoke()
        {
            var resultList = new List<TResult>();
            try
            {
                resultList.AddRange(
                    GetResults(_actionList, (a) => a()));
            }
            catch(Exception exception)
            {
                if(!HandleException(exception))
                    throw;
            }

            return resultList;
        }

        public IFluentTry<TResult> Try(Func<TResult> action)
        {
            _actionList.Add(action);
            return this;
        }

        public static new IFluentTry<TResult> Create()
        {
            return new DefaultFluentTry<TResult>(new List<Func<TResult>>(), Switch.Create<Type, Action<Exception>>());
        }

        public new IFluentTry<TResult> Catch<TException>(Action<Exception> exceptionAction)
        {
            return (IFluentTry<TResult>)base.Catch<TException>(exceptionAction);
        }

        protected IEnumerable<TResult> GetResults<TDelegate>(IList<TDelegate> delegateList, Func<TDelegate,TResult> getResult)
            where TDelegate : Delegate
        {
            var resultList = new List<TResult>();

            foreach(var action in delegateList)
                    resultList.Add(getResult(action));

            return resultList;
        }
        
        protected DefaultFluentTry(IList<Func<TResult>> actionList, ISwitch<Type, Action<Exception>> catchActionSwitch)
            : base(null, catchActionSwitch)
        {
            _actionList = actionList;
        }
    }

    internal class DefaultFluentTry<T, TResult> : DefaultFluentTry<TResult>, IFluentTry<T, TResult>
    {
        private readonly IList<Func<T, TResult>> _actionList;

        public new static IFluentTry<T, TResult> Create()
        {
            return new DefaultFluentTry<T, TResult>(new List<Func<T, TResult>>(), Switch.Create<Type, Action<Exception>>());
        }

        public IEnumerable<TResult> Invoke(T value)
        {
            var resultList = new List<TResult>();
            try
            {
                resultList.AddRange(GetResults(_actionList, a => a.Invoke(value)));
            }
            catch(Exception exception)
            {
                if(!HandleException(exception))
                    throw;
            }

            return resultList;
        }

        public IFluentTry<T, TResult> Try(Func<T, TResult> result)
        {
            _actionList.Add(result);
            return this;
        }

        protected DefaultFluentTry(IList<Func<T, TResult>> actionList, ISwitch<Type, Action<Exception>> catchActionSwitch)
            : base(null, catchActionSwitch)
        {
            _actionList = actionList;
        }
    }

    internal class DefaultFluentTryAsync : DefaultFluentTry<Task>, IFluentTryAsync
    {
        public new static IFluentTryAsync Create()
        {
            return new DefaultFluentTryAsync(new List<Func<Task>>(), Switch.Create<Type, Action<Exception>>());
        }

        public async Task InvokeAsync()
        {
            try
            {
                await Task.WhenAll(Invoke());
            }
            catch(Exception exception)
            {
                if(!HandleException(exception))
                    throw;
            }
        }

        public new IFluentTryAsync Catch<TException>(Action<Exception> exceptionAction)
        {
            return(IFluentTryAsync)base.Catch<TException>(exceptionAction);
        }

        public new IFluentTryAsync Try(Func<Task> result)
        {
            return(IFluentTryAsync)base.Try(result);
        }

        private DefaultFluentTryAsync(IList<Func<Task>> actionList, ISwitch<Type, Action<Exception>> catchActionSwitch)
            : base(actionList, catchActionSwitch)
        {

        }
    }

    internal class DefaultFluentTryAsync<T, TResult> : DefaultFluentTry<T, Task<TResult>>, IFluentTryAsync<T, TResult>
    {
        public DefaultFluentTryAsync(IList<Func<T, Task<TResult>>> actionList, ISwitch<Type, Action<Exception>> catchActionSwitch)
            : base(actionList, catchActionSwitch)
        {

        }

        public async Task<IEnumerable<TResult>> InvokeAsync(T value)
        {
            try
            {
                return await Task.WhenAll(Invoke(value));
            }
            catch(Exception exception)
            {
                if(!HandleException(exception))
                    throw;
            }

            return default;
        }

        public new IFluentTryAsync<T, TResult> Catch<TException>(Action<Exception> exceptionAction)
        {
            return (IFluentTryAsync<T, TResult>)base.Catch<TException>(exceptionAction);
        }

        public new IFluentTryAsync<T, TResult> Try(Func<T, Task<TResult>> result)
        {
            return (IFluentTryAsync<T, TResult>)base.Try(result);
        }

        public new static IFluentTryAsync<T, TResult> Create()
        {
            return new DefaultFluentTryAsync<T, TResult>(new List<Func<T, Task<TResult>>>(), Switch.Create<Type, Action<Exception>>());
        }
    }
}
