using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Providers
{
    public interface ICryptographyProvider
    {
        TCryptographicCredentials GetCryptographicCredentials<TCryptographicCredentials>(
            string symmetricAlgorithm, IEnumerable<byte> key, IEnumerable<byte> initialVector)
            where TCryptographicCredentials : ICryptographicCredentials;
        Task<IEnumerable<byte>> Encrypt(ICryptographicCredentials cryptographicCredentials, string value);
        Task<string> Decrypt(ICryptographicCredentials cryptographicCredentials, IEnumerable<byte> value);
    }
}
