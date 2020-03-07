using DNI.Core.Contracts;
using DNI.Core.Contracts.Managers;
using MessagePack;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNI.Core.Services
{
    internal sealed class DefaultMessagePackService : IMessagePackService
    {
        private readonly IMemoryStreamManager _memoryStreamManager;

        public async Task<T> Deserialise<T>(IEnumerable<byte> value, MessagePackSerializerOptions options)
        {

            using(var memoryStream = _memoryStreamManager.GetStream(true, value.ToArray()))
                return await MessagePackSerializer.DeserializeAsync<T>(memoryStream, options);
        }

        public async Task<IEnumerable<byte>> Serialise<T>(T value, MessagePackSerializerOptions options)
        {
            using(var memoryStream = _memoryStreamManager.GetStream(true))
            {
                await MessagePackSerializer.SerializeAsync<T>(memoryStream, value, options);
                return memoryStream.ToArray();
            }
        }

        public DefaultMessagePackService(IMemoryStreamManager memoryStreamManager)
        {
            _memoryStreamManager = memoryStreamManager;
        }
    }
}
