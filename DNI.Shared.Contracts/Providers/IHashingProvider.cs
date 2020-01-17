using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Providers
{
    public interface IHashingProvider
    {
        IEnumerable<byte> HashBytes(string hashName, IEnumerable<byte> bytes);
        IEnumerable<byte> PasswordDerivedBytes(string password, IEnumerable<byte> salt, int iteration, int totalNumberOfBytes);
    }
}
