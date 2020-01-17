using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts
{
    public interface ITry
    {
        void Invoke();
        ITry Try(Action action);
        ITry Catch<TException>(Action<Exception> exceptionAction);
    }

    public interface ITry<TResult> : ITry
    {
        new IEnumerable<TResult> Invoke();
        ITry<TResult> Try(Func<TResult> result);
        new ITry<TResult> Catch<TException>(Action<Exception> exceptionAction);
    }

    public interface ITry<T, TResult> : ITry<TResult>
    {
        TResult Invoke(T value);
        ITry Try(Func<T, TResult> result);
    }
}
