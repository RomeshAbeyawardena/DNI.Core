using DNI.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    internal class DefaultTry : ITry
    {
        private IList<Action> _actionList;
        private ISwitch<Type, Action<Exception>> _catchActionSwitch;

        public ITry Catch<TException>(Action<Exception> exceptionAction)
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

        public ITry Try(Action action)
        {
            _actionList.Add(action);
            return this;
        }

        public static ITry Create()
        {
            return new DefaultTry(new List<Action>(), Switch.Create<Type, Action<Exception>>());
        }

        protected bool HandleException(Exception exception)
        {
            var exceptionAction = _catchActionSwitch.Case(exception.GetType());

            if(exceptionAction == null)
                return false;

            exceptionAction.Invoke(exception);
            return true;
        }

        protected DefaultTry(IList<Action> actionList, ISwitch<Type, Action<Exception>> catchActionSwitch)
        {
            _actionList = actionList;
            _catchActionSwitch = catchActionSwitch;
        }
    }

    internal class DefaultTry<TResult> : DefaultTry, ITry<TResult>
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

        public ITry<TResult> Try(Func<TResult> action)
        {
            _actionList.Add(action);
            return this;
        }

        public static new ITry<TResult> Create()
        {
            return new DefaultTry<TResult>(new List<Func<TResult>>(), Switch.Create<Type, Action<Exception>>());
        }

        public new ITry<TResult> Catch<TException>(Action<Exception> exceptionAction)
        {
            return (ITry<TResult>)base.Catch<TException>(exceptionAction);
        }

        protected IEnumerable<TResult> GetResults<TDelegate>(IList<TDelegate> delegateList, Func<TDelegate,TResult> getResult)
            where TDelegate : Delegate
        {
            var resultList = new List<TResult>();

            foreach(var action in delegateList)
                    resultList.Add(getResult(action));

            return resultList;
        }
        
        protected DefaultTry(IList<Func<TResult>> actionList, ISwitch<Type, Action<Exception>> catchActionSwitch)
            : base(null, catchActionSwitch)
        {
            _actionList = actionList;
        }
    }

    internal class DefaultTry<T, TResult> : DefaultTry<TResult>, ITry<T, TResult>
    {
        private readonly IList<Func<T, TResult>> _actionList;

        public new static ITry<T, TResult> Create()
        {
            return new DefaultTry<T, TResult>(new List<Func<T, TResult>>(), Switch.Create<Type, Action<Exception>>());
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

        public ITry<T, TResult> Try(Func<T, TResult> result)
        {
            _actionList.Add(result);
            return this;
        }

        protected DefaultTry(IList<Func<T, TResult>> actionList, ISwitch<Type, Action<Exception>> catchActionSwitch)
            : base(null, catchActionSwitch)
        {
            _actionList = actionList;
        }
    }

    internal class DefaultTryAsync : DefaultTry<Task>, ITryAsync
    {
        public new static ITryAsync Create()
        {
            return new DefaultTryAsync(new List<Func<Task>>(), Switch.Create<Type, Action<Exception>>());
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

        public new ITryAsync Catch<TException>(Action<Exception> exceptionAction)
        {
            return(ITryAsync)base.Catch<TException>(exceptionAction);
        }

        public new ITryAsync Try(Func<Task> result)
        {
            return(ITryAsync)base.Try(result);
        }

        private DefaultTryAsync(IList<Func<Task>> actionList, ISwitch<Type, Action<Exception>> catchActionSwitch)
            : base(actionList, catchActionSwitch)
        {

        }
    }

    internal class DefaultTryAsync<T, TResult> : DefaultTry<T, Task<TResult>>, ITryAsync<T, TResult>
    {
        public DefaultTryAsync(IList<Func<T, Task<TResult>>> actionList, ISwitch<Type, Action<Exception>> catchActionSwitch)
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

        public new ITryAsync<T, TResult> Catch<TException>(Action<Exception> exceptionAction)
        {
            return (ITryAsync<T, TResult>)base.Catch<TException>(exceptionAction);
        }

        public new ITryAsync<T, TResult> Try(Func<T, Task<TResult>> result)
        {
            return (ITryAsync<T, TResult>)base.Try(result);
        }

        public new static ITryAsync<T, TResult> Create()
        {
            return new DefaultTryAsync<T, TResult>(new List<Func<T, Task<TResult>>>(), Switch.Create<Type, Action<Exception>>());
        }
    }
}
