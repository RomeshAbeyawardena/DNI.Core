using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Providers
{
    /// <summary>
    /// Represents a CryptographyProvider to encrypt or decrypt data
    /// </summary>
    public interface ICryptographyProvider
    {
        /// <summary>
        /// Generates TCryptographicCredentials from a set of specified parameters
        /// </summary>
        /// <typeparam name="TCryptographicCredentials"></typeparam>
        /// <param name="symmetricAlgorithm"></param>
        /// <param name="key"></param>
        /// <param name="initialVector"></param>
        /// <param name="initialVectorSize"></param>
        /// <returns></returns>
        TCryptographicCredentials GetCryptographicCredentials<TCryptographicCredentials>(
            string symmetricAlgorithm, IEnumerable<byte> key, IEnumerable<byte> initialVector, int initialVectorSize = 16)
            where TCryptographicCredentials : ICryptographicCredentials;

        /// <summary>
        /// Generates TCryptographicCredentials from a set of specified parameters
        /// </summary>
        /// <typeparam name="TCryptographicCredentials"></typeparam>
        /// <param name="keyDerivationPrf"></param>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="iterations"></param>
        /// <param name="totalNumberOfBytes"></param>
        /// <param name="initialVector"></param>
        /// <returns></returns>
        TCryptographicCredentials GetCryptographicCredentials<TCryptographicCredentials>(
            KeyDerivationPrf keyDerivationPrf, string password, IEnumerable<byte> salt, int iterations, int totalNumberOfBytes, IEnumerable<byte> initialVector)
            where TCryptographicCredentials : ICryptographicCredentials;

        /// <summary>
        /// Generates TCryptographicCredentials from a set of specified parameters
        /// </summary>
        /// <typeparam name="TCryptographicCredentials"></typeparam>
        /// <param name="keyDerivationPrf"></param>
        /// <param name="encoding"></param>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="iterations"></param>
        /// <param name="totalNumberOfBytes"></param>
        /// <param name="initialVector"></param>
        /// <returns></returns>
        TCryptographicCredentials GetCryptographicCredentials<TCryptographicCredentials>(
            KeyDerivationPrf keyDerivationPrf, Encoding encoding, string password, string salt, int iterations, int totalNumberOfBytes, IEnumerable<byte> initialVector)
            where TCryptographicCredentials : ICryptographicCredentials;

        /// <summary>
        /// Encrypts a textual value using specified Cryptographic credentials
        /// </summary>
        /// <param name="cryptographicCredentials"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<IEnumerable<byte>> Encrypt(ICryptographicCredentials cryptographicCredentials, string value);

        /// <summary>
        /// Decrypts an array of bytes using specified Cryptographic credentials
        /// </summary>
        /// <param name="cryptographicCredentials"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<string> Decrypt(ICryptographicCredentials cryptographicCredentials, IEnumerable<byte> value);
    }
}
