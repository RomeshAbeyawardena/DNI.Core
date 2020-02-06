using DNI.Shared.Contracts.Managers;
using Microsoft.IO;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DNI.Shared.Services
{
    public class DefaultMemoryStreamManager : IMemoryStreamManager
    {
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public MemoryStream GetStream(bool useRecyclableMemoryStreamManager = true, IEnumerable<byte> buffer = null)
        {
            MemoryStream memoryStream;
            
            memoryStream = useRecyclableMemoryStreamManager 
                ? _recyclableMemoryStreamManager.GetStream() 
                : new MemoryStream();

            if(buffer == null)
                return memoryStream;

            var bufferArray = buffer.ToArray();

            memoryStream.Write(bufferArray, 0, bufferArray.Length);
            memoryStream.Position = 0;
            return memoryStream;
        }

        public DefaultMemoryStreamManager(RecyclableMemoryStreamManager recyclableMemoryStreamManager)
        {
            _recyclableMemoryStreamManager = recyclableMemoryStreamManager;
        }
    }
}
