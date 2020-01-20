using System.Collections.Generic;
using System.IO;

namespace DNI.Shared.Contracts.Managers
{
    public interface IMemoryStreamManager
    {
        MemoryStream GetStream(bool useRecyclableMemoryStreamManager = true, IEnumerable<byte> buffer = null);
    }
}
