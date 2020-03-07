using MessagePack;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts
{
    public interface IMessagePackService
    {
        Task<IEnumerable<byte>> Serialise<T>(T value, MessagePackSerializerOptions options);
        Task<T> Deserialise<T>(IEnumerable<byte> value, MessagePackSerializerOptions options);
    }
}
