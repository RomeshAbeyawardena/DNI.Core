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
        public static void Use<TDisposable>(Action<TDisposable> useAction, Func<TDisposable> instanceCreationAction = null, params object[] constructorArguments)
            where TDisposable : IDisposable
        {
            using var disposable = CreateInstance(instanceCreationAction, constructorArguments);
            useAction(disposable);
        }
        /// <summary>
        /// Creates an IDisposable instance used to invoke a delegate between an using block
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TDisposable"></typeparam>
        /// <param name="useAction">Delegate to run within an using block</param>
        /// <param name="constructorArguments">Parameters to instantiate TDisposable</param>
        /// <returns></returns>
        public static TResult Use<TResult, TDisposable>(Func<TDisposable, TResult> useAction, Func<TDisposable> instanceCreationAction = null, params object[] constructorArguments)
            where TDisposable : IDisposable
        {
            using var disposable = CreateInstance(instanceCreationAction, constructorArguments);
            return useAction(disposable);
        }
        /// <summary>
        /// Creates an IDisposable instance used to invoke an async delegate between an using block
        /// </summary>
        /// <typeparam name="TDisposable"></typeparam>
        /// <param name="useAction">Async delegate to run within an using block</param>
        /// <param name="constructorArguments">Parameters to instantiate TDisposable</param>
        /// <returns></returns>
        public static async Task UseAsync<TDisposable>(Func<TDisposable, Task> useAction, Func<TDisposable> instanceCreationAction = null, params object[] constructorArguments)
            where TDisposable : IDisposable
        {
            using var disposable = CreateInstance(instanceCreationAction, constructorArguments);
            await useAction(disposable);
        }

        /// <summary>
        /// Creates an IDisposable instance used to invoke an async delegate between an using block
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TDisposable"></typeparam>
        /// <param name="useAction">Async delegate to run within an using block</param>
        /// <param name="constructorArguments">Parameters to instantiate TDisposable</param>
        /// <returns></returns>
        public static async Task<TResult> UseAsync<TResult, TDisposable>(Func<TDisposable, Task<TResult>> useAction, Func<TDisposable> instanceCreationAction = null, params object[] constructorArguments)
            where TDisposable : IDisposable
        {
            using var disposable = CreateInstance(instanceCreationAction, constructorArguments);
            return await useAction(disposable);
        }


        private static TDisposable CreateInstance<TDisposable>(Func<TDisposable> instanceCreationAction = null,  params object[] constructorArguments)
        {
            return instanceCreationAction == null 
                ? (TDisposable)Activator.CreateInstance(typeof(TDisposable), constructorArguments)
                : instanceCreationAction();
        }
    }
}
