using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    public interface IMessagePackService
    {
        Task<IEnumerable<byte>> Serialise<T>(T value, MessagePackSerializerOptions options);
        Task<T> Deserialise<T>(IEnumerable<byte> value, MessagePackSerializerOptions options);
    }
}
