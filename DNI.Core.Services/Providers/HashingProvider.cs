namespace DNI.Core.Services.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using DNI.Core.Contracts.Providers;
    using DNI.Core.Services.Extensions;
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;

    internal sealed class HashingProvider : IHashingProvider
    {
        public IEnumerable<byte> GetRandomNumberGeneratorBytes(int length)
        {
            var buffer = new byte[length];
            DisposableHelper.Use(randomNumberGenerator => randomNumberGenerator.GetBytes(buffer), () => RandomNumberGenerator.Create());
            return buffer;
        }

        public IEnumerable<byte> HashBytes(string hashName, IEnumerable<byte> bytes)
        {
            return DisposableHelper
                    .Use(
                        hashAlgorithm => hashAlgorithm.ComputeHash(bytes.ToArray()),
                        () => HashAlgorithm.Create(hashName));
        }

        public IEnumerable<byte> PasswordDerivedBytes(
            string password,
            IEnumerable<byte> salt,
            KeyDerivationPrf keyDerivationPrf,
            int iteration,
            int totalNumberOfBytes)
        {
            return KeyDerivation.Pbkdf2(password, salt.ToArray(), keyDerivationPrf, iteration, totalNumberOfBytes);
        }
    }
}
