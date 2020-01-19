using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts
{
    public interface IFluentTry
    {
        /// <summary>
        /// Invokes all registered delegates in a try catch block
        /// </summary>
        void Invoke();
        /// <summary>
        /// Registers a delegate to run in a try block
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IFluentTry Try(Action action);
        /// <summary>
        /// Registers an exception handler
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="exceptionAction"></param>
        /// <returns></returns>
        IFluentTry Catch<TException>(Action<Exception> exceptionAction, bool continueOnExceptionThrown = false);
    }

    public interface IFluentTry<TResult> : IFluentTry
    {
        /// <summary>
        /// Invokes all registered delegates in a try catch block.  The operation will abort on either an unhandled or handled exception.
        /// </summary>
        /// <returns></returns>
        new IEnumerable<TResult> Invoke();
        /// <summary>
        /// Registers a delegate to run in a try block
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        IFluentTry<TResult> Try(Func<TResult> result);
        /// <summary>
        /// Registers an exception handler
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="exceptionAction"></param>
        /// <returns></returns>
        new IFluentTry<TResult> Catch<TException>(Action<Exception> exceptionAction, bool continueOnExceptionThrown  = false);
    }

    public interface IFluentTry<T, TResult> : IFluentTry<TResult>
    {
        IEnumerable<TResult> Invoke(T value);
        IFluentTry<T, TResult> Try(Func<T, TResult> result);
        new IFluentTry<T, TResult> Catch<TException>(Action<Exception> exceptionAction, bool continueOnExceptionThrown  = false);
    }

    public interface IFluentTryAsync : IFluentTry<Task>
    {
        /// <summary>
        /// Invokes all registered delegates async in a try catch block.  The operation will abort on either an unhandled or handled exception.
        /// </summary>
        /// <returns></returns>
        Task InvokeAsync();
        /// <summary>
        /// Registers an async delegate to run in a try block
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        new IFluentTryAsync Try(Func<Task> result);
        /// <summary>
        /// Registers an exception handler
        /// </summary>
        /// <typeparam name="TException">Represents the exception type to handle</typeparam>
        /// <param name="exceptionAction"></param>
        /// <returns></returns>
        new IFluentTryAsync Catch<TException>(Action<Exception> exceptionAction, bool continueOnExceptionThrown  = false);
    }

    public interface IFluentTryAsync<T, TResult> : IFluentTry<T, Task<TResult>>
    {
        /// <summary>
        /// Invokes all registered delegates in a try catch block
        /// </summary>
        /// <param name="value">The parameter to be passed to all delegates</param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> InvokeAsync(T value);
        /// <summary>
        /// Registers an async delegate to run in a try block. The operation will abort on either an unhandled or handled exception.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        new IFluentTryAsync<T, TResult> Try(Func<T, Task<TResult>> result);
        /// <summary>
        /// Registers an exception handler
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="exceptionAction"></param>
        /// <returns></returns>
        new IFluentTryAsync<T, TResult> Catch<TException>(Action<Exception> exceptionAction, bool continueOnExceptionThrown  = false);
    }

    public interface IFluentTryAsync<TResult> : IFluentTry<Task<TResult>>
    {
        Task<IEnumerable<TResult>> InvokeAsync();
        new IFluentTryAsync<TResult> Try(Func<Task<TResult>> result);
        new IFluentTryAsync<TResult> Catch<TException>(Action<Exception> exceptionAction, bool continueOnExceptionThrown  = false);
    }
}
