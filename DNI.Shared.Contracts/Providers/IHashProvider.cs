using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Providers
{
    public interface IHashProvider
    {
        IEnumerable<byte> HashBytes(IEnumerable<byte> bytes);
    }
}
