using System;
using System.Collections.Generic;

namespace DNI.Core.Contracts.Services
{
    public interface IGuidService
    {
        Guid Generate();
        Guid Parse(IEnumerable<byte> guidBytes);
        Guid Parse(string guid);
    }
}
