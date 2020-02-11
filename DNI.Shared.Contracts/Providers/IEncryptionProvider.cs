using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Providers
{
    public interface IEncryptionProvider
    {
        /// <summary>
        /// Encrypts all encrypted keys
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<TResult> Encrypt<T, TResult>(T value);

        /// <summary>
        /// Decrypts all encryptable keys
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<TResult> Decrypt<T, TResult>(T value);

        Task<IEnumerable<TResult>> Encrypt<T, TResult>(IEnumerable<T> value);

        Task<IEnumerable<TResult>> Decrypt<T, TResult>(IEnumerable<T> value);
    }
}
