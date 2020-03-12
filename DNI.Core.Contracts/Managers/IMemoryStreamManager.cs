using System.Collections.Generic;
using System.IO;

namespace DNI.Core.Contracts.Managers
{
    /// <summary>
    /// Represents a manager to manage streams
    /// </summary>
    public interface IMemoryStreamManager
    {
        /// <summary>
        /// Retrieves a usable stream using a recycleable memory stream manager
        /// </summary>
        /// <param name="useRecyclableMemoryStreamManager"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        MemoryStream GetStream(bool useRecyclableMemoryStreamManager = true, IEnumerable<byte> buffer = null);
    }
}
