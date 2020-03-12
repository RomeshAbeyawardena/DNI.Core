using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Providers
{
    public interface IEncryptionProvider
    {
        /// <summary>
        /// Encrypts a value of T using reflection or decorated EncryptAttributes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<TResult> Encrypt<T, TResult>(T value);

        /// <summary>
        /// Decrypts a value of T using reflection or decorated EncryptAttributes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<TResult> Decrypt<T, TResult>(T value);

        /// <summary>
        /// Transforms a value of T to TResult using Automapper whilst encrypting decorated EncryptAttributes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> Encrypt<T, TResult>(IEnumerable<T> value);

        /// <summary>
        /// Transforms a value of T to TResult using Automapper whilst decrypting decorated EncryptAttributes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> Decrypt<T, TResult>(IEnumerable<T> value);
    }
}
