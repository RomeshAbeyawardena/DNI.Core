namespace DNI.Core.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MessagePack;

    public interface IMessagePackService
    {
        Task<IEnumerable<byte>> Serialise<T>(T value, MessagePackSerializerOptions options);

        Task<T> Deserialise<T>(IEnumerable<byte> value, MessagePackSerializerOptions options);
    }
}
