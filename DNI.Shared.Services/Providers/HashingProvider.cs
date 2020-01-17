using DNI.Shared.Contracts.Providers;
using DNI.Shared.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace DNI.Shared.Services.Providers
{
    public class HashingProvider : IHashingProvider
    {
        public IEnumerable<byte> HashBytes(string hashName, IEnumerable<byte> bytes)
        {
            return DisposableHelper
                    .Use(sha512 => sha512.ComputeHash(bytes.ToArray()), 
                            () => HashAlgorithm.Create(hashName));
        }

        public IEnumerable<byte> PasswordDerivedBytes(string password, IEnumerable<byte> salt, int iteration, int totalNumberOfBytes)
        {
            return KeyDerivation.Pbkdf2(password, salt.ToArray(), KeyDerivationPrf.HMACSHA512, iteration, totalNumberOfBytes);
        }
    }
}
