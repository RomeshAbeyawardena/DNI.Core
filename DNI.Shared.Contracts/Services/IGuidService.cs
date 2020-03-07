using System;
using System.Collections.Generic;

namespace DNI.Shared.Contracts.Services
{
    public interface IGuidService
    {
        Guid Generate();
        Guid Parse(IEnumerable<byte> guidBytes);
        Guid Parse(string guid);
    }
}
