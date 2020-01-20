using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Providers
{
    public interface ICryptographyProvider
    {
        
        TCryptographicCredentials GetCryptographicCredentials<TCryptographicCredentials>(
            string symmetricAlgorithm, IEnumerable<byte> key, IEnumerable<byte> initialVector, int initialVectorSize = 16)
            where TCryptographicCredentials : ICryptographicCredentials;
        TCryptographicCredentials GetCryptographicCredentials<TCryptographicCredentials>(
            KeyDerivationPrf keyDerivationPrf, string password, IEnumerable<byte> salt, int iterations, int totalNumberOfBytes, IEnumerable<byte> initialVector)
            where TCryptographicCredentials : ICryptographicCredentials;
        TCryptographicCredentials GetCryptographicCredentials<TCryptographicCredentials>(
            KeyDerivationPrf keyDerivationPrf, Encoding encoding, string password, string salt, int iterations, int totalNumberOfBytes, IEnumerable<byte> initialVector)
            where TCryptographicCredentials : ICryptographicCredentials;
        Task<IEnumerable<byte>> Encrypt(ICryptographicCredentials cryptographicCredentials, string value);
        Task<string> Decrypt(ICryptographicCredentials cryptographicCredentials, IEnumerable<byte> value);
    }
}
