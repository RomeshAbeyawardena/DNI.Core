using System.Collections.Generic;
using System.IO;

namespace DNI.Core.Contracts.Managers
{
    public interface IMemoryStreamManager
    {
        MemoryStream GetStream(bool useRecyclableMemoryStreamManager = true, IEnumerable<byte> buffer = null);
    }
}
