using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Collections.Generic;

namespace DNI.Core.Contracts.Providers
{
    /// <summary>
    /// Represents a Hashing provider used for hashing and providing values usuable by encryption providers
    /// </summary>
    public interface IHashingProvider
    {
        /// <summary>
        /// Hashes a string using a specified hashing algorithm
        /// </summary>
        /// <param name="hashName"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        IEnumerable<byte> HashBytes(string hashName, IEnumerable<byte> bytes);
        /// <summary>
        /// Generates password derive bytes using a common salt and hashing algorithm
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="keyDerivationPrf"></param>
        /// <param name="iteration"></param>
        /// <param name="totalNumberOfBytes"></param>
        /// <returns></returns>
        IEnumerable<byte> PasswordDerivedBytes(string password, IEnumerable<byte> salt, 
            KeyDerivationPrf keyDerivationPrf, int iteration, int totalNumberOfBytes);
        /// <summary>
        /// Generates a series of secure random numbers as a byte array
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        IEnumerable<byte> GetRandomNumberGeneratorBytes(int length);
    }
}
