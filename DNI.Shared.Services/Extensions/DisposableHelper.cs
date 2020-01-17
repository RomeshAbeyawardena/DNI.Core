using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Extensions
{
    public static class DisposableHelper
    {
        public static void Use<TDisposable>(Action<TDisposable> useAction, params object[] constructorArguments)
            where TDisposable : IDisposable
        {
            using(var disposable = CreateInstance<TDisposable>(constructorArguments))
            {
                useAction(disposable);
            }
        }

        public static TResult Use<TResult, TDisposable>(Func<TDisposable, TResult> useAction, params object[] constructorArguments)
            where TDisposable : IDisposable
        {
            using(var disposable = CreateInstance<TDisposable>(constructorArguments))
            {
                return useAction(disposable);
            }
        }

        public static async Task UseAsync<TDisposable>(Func<TDisposable, Task> useAction, params object[] constructorArguments)
            where TDisposable : IDisposable
        {
            using(var disposable = CreateInstance<TDisposable>(constructorArguments))
            {
                await useAction(disposable);
            }
        }

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
