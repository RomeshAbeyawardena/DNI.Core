namespace DNI.Core.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using DNI.Core.Contracts.Managers;
    using Microsoft.IO;

    internal sealed class DefaultMemoryStreamManager : IMemoryStreamManager
    {
        private readonly RecyclableMemoryStreamManager recyclableMemoryStreamManager;

        public MemoryStream GetStream(bool useRecyclableMemoryStreamManager = true, IEnumerable<byte> buffer = null)
        {
            MemoryStream memoryStream;

            memoryStream = useRecyclableMemoryStreamManager
                ? recyclableMemoryStreamManager.GetStream()
                : new MemoryStream();

            if (buffer == null)
            {
                return memoryStream;
            }

            var bufferArray = buffer.ToArray();

            memoryStream.Write(bufferArray, 0, bufferArray.Length);
            memoryStream.Position = 0;
            return memoryStream;
        }

        public DefaultMemoryStreamManager(RecyclableMemoryStreamManager recyclableMemoryStreamManager)
        {
            this.recyclableMemoryStreamManager = recyclableMemoryStreamManager;
        }
    }
}
