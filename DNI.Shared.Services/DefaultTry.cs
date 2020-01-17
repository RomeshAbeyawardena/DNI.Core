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

        public new IEnumerable<TResult> Invoke()
        {
            var resultList = new List<TResult>();
            try
            {
                foreach(var action in _actionList)
                    resultList.Add(action.Invoke());

                return resultList;
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

        private DefaultTry(IList<Func<TResult>> actionList, ISwitch<Type, Action<Exception>> catchActionSwitch)
            : base(null, catchActionSwitch)
        {
            _actionList = actionList;
        }
    }
}
