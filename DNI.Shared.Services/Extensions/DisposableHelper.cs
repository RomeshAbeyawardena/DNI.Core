using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Extensions
{
    /// <summary>
    /// A helper that encapsulate logic between an using block.
    /// </summary>
    public static class DisposableHelper
    {
        /// <summary>
        /// Creates an IDisposable instance used to invoke a delegate between an using block
        /// </summary>
        /// <typeparam name="TDisposable"></typeparam>
        /// <param name="useAction">Delegate to run within an using block</param>
        /// <param name="constructorArguments">Parameters to instantiate TDisposable</param>
        public static void Use<TDisposable>(Action<TDisposable> useAction, params object[] constructorArguments)
            where TDisposable : IDisposable
        {
            using(var disposable = CreateInstance<TDisposable>(constructorArguments))
            {
                useAction(disposable);
            }
        }
        /// <summary>
        /// Creates an IDisposable instance used to invoke a delegate between an using block
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TDisposable"></typeparam>
        /// <param name="useAction">Delegate to run within an using block</param>
        /// <param name="constructorArguments">Parameters to instantiate TDisposable</param>
        /// <returns></returns>
        public static TResult Use<TResult, TDisposable>(Func<TDisposable, TResult> useAction, params object[] constructorArguments)
            where TDisposable : IDisposable
        {
            using(var disposable = CreateInstance<TDisposable>(constructorArguments))
            {
                return useAction(disposable);
            }
        }
        /// <summary>
        /// Creates an IDisposable instance used to invoke an async delegate between an using block
        /// </summary>
        /// <typeparam name="TDisposable"></typeparam>
        /// <param name="useAction">Async delegate to run within an using block</param>
        /// <param name="constructorArguments">Parameters to instantiate TDisposable</param>
        /// <returns></returns>
        public static async Task UseAsync<TDisposable>(Func<TDisposable, Task> useAction, params object[] constructorArguments)
            where TDisposable : IDisposable
        {
            using(var disposable = CreateInstance<TDisposable>(constructorArguments))
            {
                await useAction(disposable);
            }
        }

        /// <summary>
        /// Creates an IDisposable instance used to invoke an async delegate between an using block
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TDisposable"></typeparam>
        /// <param name="useAction">Async delegate to run within an using block</param>
        /// <param name="constructorArguments">Parameters to instantiate TDisposable</param>
        /// <returns></returns>
        public static async Task<TResult> UseAsync<TResult, TDisposable>(Func<TDisposable, Task<TResult>> useAction, params object[] constructorArguments)
            where TDisposable : IDisposable
        {
            using(var disposable = CreateInstance<TDisposable>(constructorArguments))
            {
                return await useAction(disposable);
            }
        }


        private static TDisposable CreateInstance<TDisposable>(params object[] constructorArguments)
        {
            return (TDisposable)Activator.CreateInstance(typeof(TDisposable), constructorArguments);
        }
    }
}
