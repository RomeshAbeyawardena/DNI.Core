using DNI.Shared.Contracts.Managers;
using Microsoft.IO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    public class MemoryStreamManager : IMemoryStreamManager
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

        public MemoryStreamManager(RecyclableMemoryStreamManager recyclableMemoryStreamManager)
        {
            _recyclableMemoryStreamManager = recyclableMemoryStreamManager;
        }
    }
}
