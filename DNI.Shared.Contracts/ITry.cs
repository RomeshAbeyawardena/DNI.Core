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
        IEnumerable<TResult> Invoke(T value);
        ITry<T, TResult> Try(Func<T, TResult> result);
    }

    public interface ITryAsync : ITry<Task>
    {
        Task InvokeAsync();
        new ITryAsync Try(Func<Task> result);
        new ITryAsync Catch<TException>(Action<Exception> exceptionAction);
    }
}
