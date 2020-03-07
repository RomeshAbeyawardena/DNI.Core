using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Collections.Generic;

namespace DNI.Core.Contracts.Providers
{
    public interface IHashingProvider
    {
        IEnumerable<byte> HashBytes(string hashName, IEnumerable<byte> bytes);
        IEnumerable<byte> PasswordDerivedBytes(string password, IEnumerable<byte> salt, 
            KeyDerivationPrf keyDerivationPrf, int iteration, int totalNumberOfBytes);
        IEnumerable<byte> GetRandomNumberGeneratorBytes(int length);
    }
}
