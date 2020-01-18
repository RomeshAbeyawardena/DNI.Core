using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Managers
{
    public interface IMemoryStreamManager
    {
        MemoryStream GetStream(bool useRecyclableMemoryStreamManager = true, IEnumerable<byte> buffer = null);
    }
}
