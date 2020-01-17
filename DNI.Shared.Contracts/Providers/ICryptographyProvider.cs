using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Providers
{
    public interface ICryptographyProvider
    {
        IEnumerable<byte> Encrypt(ICryptographicCredentials cryptographicCredentials, IEnumerable<byte> key, string value);
        string Decrypt(ICryptographicCredentials cryptographicCredentials, IEnumerable<byte> key, IEnumerable<byte> value);
    }
}
