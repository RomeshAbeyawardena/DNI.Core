using DNI.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    /// <summary>
    /// Represents a static factory method to create instances of FluentTry
    /// </summary>
    public static class FluentTry
    {
        /// <summary>
        /// Creates a new instance of IFluentTry
        /// </summary>
        /// <returns>An instance of IFluentTry</returns>
        public static IFluentTry Create()
        {
            return DefaultFluentTry
                .Create();
        }

        /// <summary>
        /// Creates a new instance of IFluentTry<TResult>
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns>An instance of IFluentTry</returns>
        public static IFluentTry<TResult> Create<TResult>()
        {
            return DefaultFluentTry<TResult>
                .Create();
        }
        /// <summary>
        /// Creates a new instance of IFluentTry
        /// </summary>
        /// <returns>An async instance of IFluentTry</returns>
        public static IFluentTryAsync CreateAsync()
        {
            return DefaultFluentTryAsync
                .Create();
        }

        public static IFluentTryAsync<TResult> CreateAsync<TResult>()
        {
            return DefaultFluentTryAsync<TResult>
                .Create();
        }

        public static IFluentTryAsync<T, TResult> CreateAsync<T, TResult>()
        {
            return DefaultFluentTryAsync<T, TResult>
                .Create();
        }
    }
}
